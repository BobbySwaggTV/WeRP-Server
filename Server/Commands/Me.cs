using Aeonix;
using CitizenFX.Core;
using System;
using System.Collections.Generic;

namespace Server.Commands
{
	class Me : CommandBase
	{
		public Me()
		{
			this.Init("Me", "Outputs the specified text as an action");
		}

		public override bool Process(List<String> args = null)
		{
			String message = "";

			foreach(String arg in args)
			{
				message += " " + arg;
			}
			
			CommandHandlerBase handler = this.GetHandler();
			String playerName = handler.GetExecutorName();
			BaseScript.TriggerClientEvent("chatMessage", "", new int[] { 255, 255, 25 }, "^5" + playerName + message + "^0");
			return false;
		}
	}
}
