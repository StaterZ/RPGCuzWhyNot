using System;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data;
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
	}
}