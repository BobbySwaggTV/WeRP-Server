using Aeonix;
using CitizenFX.Core;
using System;
using System.Collections.Generic;

namespace Server.Commands
{
	class Rules : CommandBase
	{
		public Rules()
		{
			this.Init("Rules", "Outputs a list of our rules");
		}

		public List<String> GetRules()
		{
			return new List<String>
			{
				"Have fun and create an enjoyable experience for all players/staff",
				"Be respectful to ^5all^0 players",
				"If you have an issue with a player, contact an admin via Discord"
			};
		}

		public override bool Process(List<String> args = null)
		{
			String message = "The following are the rules for ^5WeRP^0:";
			List<String> rules = this.GetRules();

			foreach (String rule in rules)
			{
				message += "\n › " + rule;
			}

			CommandHandlerBase handler = this.GetHandler();
			String playerName = handler.GetExecutorName();
			BaseScript.TriggerClientEvent("chatMessage", "", new int[] { 255, 255, 255 }, message);
			return false;
		}
	}
}
