using Aeonix;
using CitizenFX.Core;
using System;
using System.Collections.Generic;

namespace Server.Commands
{
	class DeleteVehicle : CommandBase
	{
		public DeleteVehicle()
		{
			this.Init("Delete Vehicle", "Deletes the vehicle the player is in");
		}

		public override bool Process(List<String> args = null)
		{
			BaseScript.TriggerClientEvent("werp:deleteVehicle", 0);
			return true;
		}
	}
}
