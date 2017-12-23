using Aeonix;
using CitizenFX.Core;
using System;
using System.Collections.Generic;

namespace Server.Commands
{
	class Help : CommandBase
	{
		public Help()
		{
			this.Init("Help", "Outputs a list of all available commands and how to use them");
		}

		public String GetHelpContent()
		{
			Dictionary<String, CommandBase> commands = Core.GetInstance().GetCommands();
			String helpMessage = "The following commands can be ran:";

			foreach(KeyValuePair<String, CommandBase> data in commands)
			{
				String name = data.Value.GetName().ToLower();
				String description = data.Value.GetDescription();
				String nameAppend = "› ^5" + name + "^0";

				if (description != "")
				{
					nameAppend += " - " + description;
				}

				helpMessage += "\n" + nameAppend + "\nUsage: " + data.Value.GetUsageResponse();
			}

			return helpMessage;
		}

		public override bool Process(List<String> args = null)
		{
			BaseScript.TriggerClientEvent("chatMessage", "[System]", new int[] { 255, 255, 255 }, this.GetHelpContent());
			return false;
		}
	}
}
