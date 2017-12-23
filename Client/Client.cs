using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Threading.Tasks;

namespace Client
{
	public class Client : BaseScript
	{
		public Client()
		{
			this.LoadModules();
			this.LoadEvents();

			bool monthIsDecember = (DateTime.Now.Month == 12) ? true : false;
			if (monthIsDecember)
			{
				Debug.WriteLine("Christmas time, bitches!");

				API.SetWeatherTypeNowPersist("XMAS");
				API.SetForcePedFootstepsTracks(true);
				API.SetForceVehicleTrails(true);
			}
			else
			{
				Debug.WriteLine("The Grinch stole Christmas :(");
			}

			Tick += OnTick;
		}

		public void DeleteVehicle(int player = -1)
		{
			Debug.WriteLine("Deleting vehicle..");

			int playerPed = API.GetPlayerPed(player);
			int vehicle = API.GetVehiclePedIsIn(playerPed, false);

			if (vehicle == 0)
			{
				TriggerEvent("chatMessage", "^1[System]", new int[] { 255, 255, 255 }, "You must be in a vehicle to use that command!");
			}
			else
			{
				API.DeleteVehicle(ref vehicle);
				TriggerEvent("chatMessage", "^5[System]", new int[] { 255, 255, 255 }, "Vehicle successfully deleted!");
			}
		}

		public String GetPlayerDirection(int playerPed)
		{
			float entityHeading = API.GetEntityHeading(playerPed);
			String direction = "Unknown";

			if (Math.Abs(entityHeading - 0) < 22.5 || Math.Abs(entityHeading - 360) < 22.5)
			{
				direction = "North";
			}
			else if (Math.Abs(entityHeading - 45) < 22.5)
			{
				direction = "Northwest";
			}
			else if (Math.Abs(entityHeading - 90) < 22.5)
			{
				direction = "West";
			}
			else if (Math.Abs(entityHeading - 135) < 22.5)
			{
				direction = "Southwest";
			}
			else if (Math.Abs(entityHeading - 180) < 22.5)
			{
				direction = "South";
			}
			else if (Math.Abs(entityHeading - 225) < 22.5)
			{
				direction = "Southwest";
			}
			else if (Math.Abs(entityHeading - 270) < 22.5)
			{
				direction = "East";
			}
			else if (Math.Abs(entityHeading - 315) < 22.5)
			{
				direction = "Northeast";
			}

			return direction;
		}

		public String GetZoneFullname(String zone)
		{
			Dictionary<String, String> zoneDefinitions = new Dictionary<String, String>
			{
				["AIRP"] = "Los Santos International Airport",
				["ALAMO"] = "Alamo Sea",
				["ALTA"] = "Alta",
				["ARMYB"] = "Fort Zancudo",
				["BANHAMC"] = "Banham Canyon Dr",
				["BANNING"] = "Banning",
				["BEACH"] = "Vespucci Beach",
				["BHAMCA"] = "Banham Canyon",
				["BRADP"] = "Braddock Pass",
				["BRADT"] = "Braddock Tunnel",
				["BURTON"] = "Burton",
				["CALAFB"] = "Calafia Bridge",
				["CANNY"] = "Raton Canyon",
				["CCREAK"] = "Cassidy Creek",
				["CHAMH"] = "Chamberlain Hills",
				["CHIL"] = "Vinewood Hills",
				["CHU"] = "Chumash",
				["CMSW"] = "Chiliad Mountain State Wilderness",
				["CYPRE"] = "Cypress Flats",
				["DAVIS"] = "Davis",
				["DELBE"] = "Del Perro Beach",
				["DELPE"] = "Del Perro",
				["DELSOL"] = "La Puerta",
				["DESRT"] = "Grand Senora Desert",
				["DOWNT"] = "Downtown",
				["DTVINE"] = "Downtown Vinewood",
				["EAST_V"] = "East Vinewood",
				["EBURO"] = "El Burro Heights",
				["ELGORL"] = "El Gordo Lighthouse",
				["ELYSIAN"] = "Elysian Island",
				["GALFISH"] = "Galilee",
				["GOLF"] = "GWC and Golfing Society",
				["GRAPES"] = "Grapeseed",
				["GREATC"] = "Great Chaparral",
				["HARMO"] = "Harmony",
				["HAWICK"] = "Hawick",
				["HORS"] = "Vinewood Racetrack",
				["HUMLAB"] = "Humane Labs and Research",
				["JAIL"] = "Bolingbroke Penitentiary",
				["KOREAT"] = "Little Seoul",
				["LACT"] = "Land Act Reservoir",
				["LAGO"] = "Lago Zancudo",
				["LDAM"] = "Land Act Dam",
				["LEGSQU"] = "Legion Square",
				["LMESA"] = "La Mesa",
				["LOSPUER"] = "La Puerta",
				["MIRR"] = "Mirror Park",
				["MORN"] = "Morningwood",
				["MOVIE"] = "Richards Majestic",
				["MTCHIL"] = "Mount Chiliad",
				["MTGORDO"] = "Mount Gordo",
				["MTJOSE"] = "Mount Josiah",
				["MURRI"] = "Murrieta Heights",
				["NCHU"] = "North Chumash",
				["NOOSE"] = "N.O.O.S.E",
				["OCEANA"] = "Pacific Ocean",
				["PALCOV"] = "Paleto Cove",
				["PALETO"] = "Paleto Bay",
				["PALFOR"] = "Paleto Forest",
				["PALHIGH"] = "Palomino Highlands",
				["PALMPOW"] = "Palmer-Taylor Power Station",
				["PBLUFF"] = "Pacific Bluffs",
				["PBOX"] = "Pillbox Hill",
				["PROCOB"] = "Procopio Beach",
				["RANCHO"] = "Rancho",
				["RGLEN"] = "Richman Glen",
				["RICHM"] = "Richman",
				["ROCKF"] = "Rockford Hills",
				["RTRAK"] = "Redwood Lights Track",
				["SANAND"] = "San Andreas",
				["SANCHIA"] = "San Chianski Mountain Range",
				["SANDY"] = "Sandy Shores",
				["SKID"] = "Mission Row",
				["SLAB"] = "Stab City",
				["STAD"] = "Maze Bank Arena",
				["STRAW"] = "Strawberry",
				["TATAMO"] = "Tataviam Mountains",
				["TERMINA"] = "Terminal",
				["TEXTI"] = "Textile City",
				["TONGVAH"] = "Tongva Hills",
				["TONGVAV"] = "Tongva Valley",
				["VCANA"] = "Vespucci Canals",
				["VESP"] = "Vespucci",
				["VINE"] = "Vinewood",
				["WINDF"] = "Ron Alternates Wind Farm",
				["WVINE"] = "West Vinewood",
				["ZANCUDO"] = "Zancudo River",
				["ZP_ORT"] = "Port of South Los Santos",
				["ZQ_UAR"] = "Davis Quartz"
			};

			zoneDefinitions.TryGetValue(zone, out String zoneMatch);
			return zoneMatch;
		}

		private void LoadEvents()
		{
			// TODO: Load these dynamically..
			Debug.WriteLine("Registering events..");
			Debug.WriteLine("Registering event: playerSpawned");
			EventHandlers.Add("playerSpawned", new Action<ExpandoObject>(PlayerSpawned));

			Debug.WriteLine("Registering event: werp:deleteVehicle");
			EventHandlers.Add("werp:deleteVehicle", new Action<int>(DeleteVehicle));
		}

		private void LoadModules()
		{
			//Core.getInstance().loadModules(this.GetType());
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		public async Task OnTick()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
			// Watermark
			API.SetTextFont(1);
			API.SetTextProportional(true);
			API.SetTextScale(0.0F, 0.6F);
			API.SetTextColour(192, 192, 192, 255);
			API.SetTextDropshadow(0, 0, 0, 0, 255);
			API.SetTextEdge(1, 0, 1, 0, 255);
			API.SetTextDropShadow();
			API.SetTextOutline();
			API.SetTextEntry("STRING");
			API.AddTextComponentString("~b~We~w~RP");
			API.DrawText(0.005F, 0.005F);

			API.SetTextFont(4);
			API.SetTextProportional(true);
			API.SetTextScale(0.0F, 0.5F);
			API.SetTextColour(192, 192, 192, 255);
			API.SetTextDropshadow(0, 0, 0, 0, 255);
			API.SetTextEdge(1, 0, 1, 0, 255);
			API.SetTextDropShadow();
			API.SetTextOutline();
			API.SetTextEntry("STRING");
			API.AddTextComponentString("v1.0.1");
			API.DrawText(0.044F, 0.0058F);

			// Player Location Display
			// Coordinates
			int playerPed = API.GetPlayerPed(-1);
			Vector3 playerPosition = API.GetEntityCoords(playerPed, true);
			String posX = String.Format("{0:0.00}", playerPosition.X);
			String posY = String.Format("{0:0.00}", playerPosition.Y);
			String posZ = String.Format("{0:0.00}", playerPosition.Z);

			API.SetTextFont(4);
			API.SetTextScale(0.0F, 0.5F);
			API.SetTextColour(255, 255, 255, 255);
			API.SetTextDropshadow(0, 0, 0, 0, 255);
			API.SetTextEdge(1, 0, 1, 0, 255);
			API.SetTextDropShadow();
			API.SetTextOutline();
			API.SetTextEntry("STRING");
			API.AddTextComponentString("~r~X~w~: " + posX + "\n~r~Y~w~: " + posY + "\n~r~Z~w~: " + posZ);
			API.DrawText(0.005F, 0.042F);

			// Location
			String direction = this.GetPlayerDirection(playerPed);
			float safezoneSize = API.GetSafeZoneSize();
			float screenWidth = Screen.Width;
			float screenHeight = Screen.Height;
			PointF offset = new PointF((int)Math.Round((screenWidth - (screenWidth * safezoneSize)) / 2 + 1), (int)Math.Round((screenHeight - (screenHeight * safezoneSize)) / 2 - 2));

			Vector3 position = API.GetEntityCoords(playerPed, true);
			float xPosition = (0.145F + ((1.0F - safezoneSize) / 2));
			float yPosition = (0.825F - ((1.0F - safezoneSize) / 2));

			uint streetNameHash = 0;
			uint crossingRoadHash = 0;
			API.GetStreetNameAtCoord(position.X, position.Y, position.Z, ref streetNameHash, ref crossingRoadHash);

			String streetName = API.GetStreetNameFromHashKey(streetNameHash);
			String streetNameOutput = streetName;
			String crossingRoad = API.GetStreetNameFromHashKey(crossingRoadHash);
			if (crossingRoad != "")
			{
				streetNameOutput += " / " + crossingRoad;
			}

			String zoneName = this.GetZoneFullname(API.GetNameOfZone(position.X, position.Y, position.Z));

			API.SetTextFont(2);
			API.SetTextScale(0.0F, 0.9F);
			API.SetTextColour(255, 255, 255, 255);
			API.SetTextDropshadow(0, 0, 0, 0, 255);
			API.SetTextEdge(1, 0, 1, 0, 255);
			API.SetTextDropShadow();
			API.SetTextOutline();
			API.SetTextEntry("STRING");
			API.AddTextComponentString(direction);
			API.DrawText(xPosition, yPosition);

			API.SetTextFont(4);
			API.SetTextScale(0.0F, 0.6F);
			API.SetTextColour(198, 198, 198, 255);
			API.SetTextDropshadow(0, 0, 0, 0, 255);
			API.SetTextEdge(1, 0, 1, 0, 255);
			API.SetTextDropShadow();
			API.SetTextOutline();
			API.SetTextEntry("STRING");
			API.AddTextComponentString(streetNameOutput);
			API.DrawText(xPosition, yPosition + 0.047F);

			API.SetTextFont(2);
			API.SetTextScale(0.0F, 0.4F);
			API.SetTextColour(240, 200, 80, 255);
			API.SetTextDropshadow(0, 0, 0, 0, 255);
			API.SetTextEdge(1, 0, 1, 0, 255);
			API.SetTextDropShadow();
			API.SetTextOutline();
			API.SetTextEntry("STRING");
			API.AddTextComponentString(zoneName);
			API.DrawText(xPosition, yPosition + 0.085F);
		}

		private void PlayerSpawned(ExpandoObject spawn)
		{
			var model = ((IDictionary<String, Object>)spawn);
			double x = 0;
			double y = 0;
			double z = 0;

			foreach (KeyValuePair<String, Object> data in model)
			{
				if (data.Key == "x")
				{
					x = (double)data.Value;
				}
				else if (data.Key == "y")
				{
					y = (double)data.Value;
				}
				else if (data.Key == "x")
				{
					z = (double)data.Value;
				}
			}

			PlayerList players = new PlayerList();
			int playerId = Game.Player.ServerId;
			Player player = players[playerId];
			Debug.WriteLine("Player " + player.Name + "(#" + playerId + ") spawned (X: " + x + ", Y: " + y + ", Z: " + z + ")");

			bool monthIsDecember = (DateTime.Now.Month == 12) ? true : false;
			String welcomeMessage = "Welcome to ^5WeRP^0!\n › Run ^5/help^0 to see the available commands\n › ^5/rules^0 to see our rules\n › ^5/contact^0 to contact us!";
			if (monthIsDecember)
			{
				welcomeMessage = welcomeMessage.Replace("Welcome to", "^1Happy ^2Holidays^0 from");
			}

			// TODO: Don't show this message if the player is respawning.
			// TODO: Change the message to say "Welcome back" instead of "Welcome" if the player has joined before
			TriggerEvent("chatMessage", "", new int[] { 255, 255, 255 }, welcomeMessage);
		}
	}
}
