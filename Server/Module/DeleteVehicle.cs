using Aeonix;
using System;
using System.Collections.Generic;

namespace Server.Module
{
	class DeleteVehicle : ModuleBase
	{
		public DeleteVehicle()
		{
			Dictionary<String, CommandBase> commands = new Dictionary<String, CommandBase>();
			this.Init("Delete Vehicle", commands);
		}
	}
}
