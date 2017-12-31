using Aeonix;
using Aeonix.Util;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Server
{
	public class Server : BaseScript
	{
		public Server()
		{
			this.LoadModules();
			this.LoadCommands();
			this.LoadEvents();
		}

		private void ChatMessage(int playerId, String playerName, String message)
		{
			if (!message.StartsWith("/"))
			{
				return;
			}

			API.CancelEvent();

			List<String> args = new List<String>();
			String[] splitMessage = message.Replace("/", "").Split(null);
			String command = splitMessage[0];

			if (splitMessage.Length > 1)
			{
				for (int i = 1; i < splitMessage.Length; i++)
				{
					args.Add(splitMessage[i]);
				}
			}

			if (!Core.GetInstance().HasCommand(command))
			{
				TriggerClientEvent("chatMessage", "", Color.Error, "[System] Unknown command: ^0/" + command);
				return;
			}

			Assembly assembly = Assembly.GetAssembly(this.GetType());
			Type commandType = assembly.GetType("Server.Commands." + command.Substring(0, 1).ToUpper() + command.Substring(1, (command.Length - 1)));
			CommandBase commandObject = (CommandBase)Core.GetInstance().GetCommand(command);
			CommandHandlerBase commandHandler = commandObject.GetHandler();
			commandHandler.SetExecutor(playerId, playerName);
			commandHandler.SetArgs(args);
			commandObject.GetHandler().Process();
		}

		private void LoadCommands()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			List<String> commands = new List<String>();
			int registeredCommands = 0;

			foreach (TypeInfo typeInfo in assembly.DefinedTypes)
			{
				if (!typeInfo.FullName.Contains("Commands"))
				{
					continue;
				}

				commands.Add(typeInfo.FullName);
			}

			foreach (String command in commands)
			{
				String commandName = command.ToLower().Replace("server.commands.", "");
				Type commandType = assembly.GetType(command);

				if (commandType == null)
				{
					Core.LogServer("Can't create command object for '" + commandName + "', class doesn't exist..");
					continue;
				}

				CommandBase commandObject = (CommandBase)Activator.CreateInstance(commandType);
				if (commandObject == null)
				{
					Core.LogServer("Command '" + commandName + "' not registered, couldn't create instance..");
					continue;
				}

				Core.LogServer("Command registered: " + commandName);

				registeredCommands++;
				Core.GetInstance().RegisterCommand(commandName, commandObject);
			}

			if (registeredCommands == commands.Count)
			{
				Core.LogServer("All commands registered successfully!");
			}
			else
			{
				Core.LogServer("Registered " + registeredCommands + "/" + commands.Count + " successfully");
			}
		}

		private void LoadEvents()
		{
			// TODO: Load these dynamically..
			Core.LogServer("Registering events..");
			Core.LogServer("Registering event: chatMessage");
			EventHandlers.Add("chatMessage", new Action<int, String, String>(ChatMessage));

			Core.LogServer("Registering event: playerDropped");
			EventHandlers.Add("playerDropped", new Action<Player, String>(PlayerDropped));
		}

		private void LoadModules()
		{
			Core.GetInstance().LoadModules(this.GetType());
		}

		private void PlayerDropped([FromSource] Player player, String reason)
		{
			String leftMessage = player.Name + " " + reason.ToLower().Replace(".", "");
			Debug.WriteLine(leftMessage);
			TriggerClientEvent("chatMessage", "", Color.System, leftMessage);
		}
	}
}
