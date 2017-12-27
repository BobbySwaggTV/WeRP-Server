using Aeonix;
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
		private bool welcomeMessageShown = false;

		public Client()
		{
			this.LoadModules();
			this.LoadEvents();

			this.ActivateSeasonalEvent();

			Tick += OnTick;
		}

		public void ActivateSeasonalEvent()
		{
			int currentMonth = DateTime.Now.Month;

			if (currentMonth == 12) // Christmas
			{
				Debug.WriteLine("Activating Christmas seasonal event..");

				API.SetWeatherTypeNowPersist("XMAS");
				API.SetForcePedFootstepsTracks(true);
				API.SetForceVehicleTrails(true);
			}
			else
			{
				Debug.WriteLine("No seasonal effects to apply..");
			}
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

		public String GetWelcomeMessage(bool includeUsername = false)
		{
			int currentDay = DateTime.Now.Day;
			int currentMonth = DateTime.Now.Month;
			String replace = "";
			String welcomeMessage = "Welcome to ^5WeRP^0!\n › Run ^5/help^0 to see the available commands\n › ^5/rules^0 to see our rules\n › ^5/contact^0 to contact us!";

			if (currentMonth == 1 && currentDay == 1) // New Years
			{
				replace = "Happy New Years";
			}
			else if (currentMonth == 12) // Christmas
			{
				replace = "^1Happy ^2Holidays^0";
			}

			if (replace != "")
			{
				welcomeMessage = welcomeMessage.Replace("Welcome to", replace + " from");
			}

			if (includeUsername)
			{
				int playerId = Game.Player.ServerId;
				String playerName = API.GetPlayerName(API.GetPlayerFromServerId(playerId));

				welcomeMessage = welcomeMessage.Replace("^5WeRP^0", "^5WeRP^0, " + playerName);
			}

			return welcomeMessage;
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

			String zoneName = Util.GetZoneFullname(API.GetNameOfZone(position.X, position.Y, position.Z));

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

		private void PlayerSpawned(ExpandoObject spawned)
		{
			if (this.welcomeMessageShown)
			{
				return;
			}

			int playerId = API.GetPlayerIndex();
			String playerName = API.GetPlayerName(API.GetPlayerFromServerId(playerId));

			// TODO: Change the message to say "Welcome back" instead of "Welcome" if the player has joined before
			TriggerEvent("chatMessage", "", new int[] { 255, 255, 255 }, this.GetWelcomeMessage(true));
			this.welcomeMessageShown = true;
		}
	}
}
