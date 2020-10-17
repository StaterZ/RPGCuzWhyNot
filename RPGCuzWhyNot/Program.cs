using System;
using System.Collections.Generic;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.MenuSystem;
using RPGCuzWhyNot.Systems.Networking;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Things.Item;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static int Main() {
			try {
				return Run();
			} catch (Exception e) {
				Console.WriteLine(e);
				Utils.WaitForPlayer();
				return -1;
			}
		}

		private static int Run() {
			Terminal.IsCursorVisible = false; //default to not showing cursor

			TestNetworking();

			//Load content
			if (!LoadContent()) {
				return 1;
			}

			//construct player
			player = new Player(new Human(Humanoid.Gender.Male)) {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				stats = new Stats(10, 10, 10, 10)
			};


			//add start items to player
			player.Inventory.MoveItem(DataLoader.CreateItem("blue_potion"));
			player.Inventory.MoveItem(DataLoader.CreateItem("backpack"));
			player.Wielding.MoveItem((IWieldable)DataLoader.CreateItem("deluxe_debug_doodad"));
			player.Wielding.MoveItem((IWieldable)DataLoader.CreateItem("greatsword"));


			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = Utils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				player.Handle(commandText);
			}
		}

		private static bool LoadContent() { //Load content
			Terminal.WriteLineWithoutDelay("{fg:Yellow}(Loading Content...)");
			DataLoader.ErrorLevel errorLevel = DataLoader.LoadGameData();

			if (errorLevel == DataLoader.ErrorLevel.Error) {
				return false;
			}

			Terminal.WriteLineWithoutDelay("{fg:Green}(Done!)");

			if (errorLevel == DataLoader.ErrorLevel.Success) {
				Utils.Sleep(100);
				Terminal.Clear();
			}

			return true;
		}
		
		private static void TestNetworking() {
			const string ip = "127.0.0.1";
			const int port = 27015;

			Menu networkMenu = new Menu("Networking",
				new MenuItem("Server", "Start a stand alone server", ctx => {
					ctx.ExitEntireMenuStack();

					//int port = int.Parse(ConsoleUtils.Ask("Port:"));

					NetworkManager.server = new Server();
					NetworkManager.server.Start(port);
				}),
				new MenuItem("Host", "Host a server and join it", ctx => {
					ctx.ExitEntireMenuStack();

					//const string ip = "127.0.0.1";
					//int port = int.Parse(ConsoleUtils.Ask("Port:"));

					NetworkManager.server = new Server();
					NetworkManager.server.Start(port);

					NetworkManager.localClient = new LocalClient();
					NetworkManager.ConnectToServer(ip, port);
				}),
				new MenuItem("Join", "Join a server", ctx => {
					ctx.ExitEntireMenuStack();

					//string ip = ConsoleUtils.Ask("IP:");
					//int port = int.Parse(ConsoleUtils.Ask("Port:"));

					NetworkManager.localClient = new LocalClient();
					NetworkManager.ConnectToServer(ip, port);
				})
			);

			networkMenu.EnterAsRoot();

			//NetworkManager.server.SendWelcomeMessage("This is a message the has gone over the network! impressive, right?");


			List<TestPlayer> players = new List<TestPlayer>();
			void OnClientConnect(Client client) {
				TestPlayer testPlayer = new TestPlayer(client.Id);

				lock (players) {
					players.Add(testPlayer);
				}
			}

			void OnClientDisconnect(Client client) {
			}

			if (NetworkManager.server != null) {
				NetworkManager.server.OnClientConnect += OnClientConnect;
				NetworkManager.server.OnClientDisconnect += OnClientDisconnect;
			}

			if (NetworkManager.localClient != null) {
				if (NetworkManager.server == null) {
					NetworkManager.localClient.Connect();
					OnClientConnect(NetworkManager.localClient);
				} else {
					NetworkManager.server.Register(NetworkManager.localClient);
				}
			}

			while (true) {
				TestPlayer[] safePlayers;
				lock (players) {
					safePlayers = players.ToArray();
				}

				foreach (TestPlayer testPlayer in safePlayers) {
					testPlayer.Update();
				}
			}
		}
	}
}