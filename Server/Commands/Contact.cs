using Aeonix;
using Aeonix.Util;
using CitizenFX.Core;
using System;
using System.Collections.Generic;

namespace Server.Commands
{
	class Contact : CommandBase
	{
		public Contact()
		{
			this.Init("contact", "Provides ways of contacting us");
		}

		public List<String> GetContactInfo()
		{
			return new List<String>
			{
				"Discord - https://discord.gg/Q4xPFuh",
				"Facebook - https://facebook.com/werp.official/",
				"Twitter - https://twitter.com/werp_official/"
			};
		}

		public override bool Process(List<String> args = null)
		{
			List<String> contacts = this.GetContactInfo();
			String message = "Need help? Want to appeal a ban? Feel free to drop us a message:";

			foreach (String contact in contacts)
			{
				message += "\n › " + contact;
			}

			CommandHandlerBase handler = this.GetHandler();
			String playerName = handler.GetExecutorName();
			BaseScript.TriggerClientEvent("chatMessage", "", Color.Default, message);
			return false;
		}
	}
}
