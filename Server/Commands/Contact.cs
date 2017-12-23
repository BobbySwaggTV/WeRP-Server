using Aeonix;
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
				"^5Discord^0 - https://discord.gg/Q4xPFuh",
				"^5Facebook^0 - https://facebook.com/werp.official/",
				"^5Twitter^0 - https://twitter.com/werp_official/"
			};
		}

		public override bool Process(List<String> args = null)
		{
			List<String> contacts = this.GetContactInfo();
			String message = "Need help? Want to appeal a ban? Contact us using one of the following methods:";

			foreach (String contact in contacts)
			{
				message += "\n › " + contact;
			}

			CommandHandlerBase handler = this.GetHandler();
			String playerName = handler.GetExecutorName();
			BaseScript.TriggerClientEvent("chatMessage", "", new int[] { 255, 255, 255 }, message);
			return false;
		}
	}
}
