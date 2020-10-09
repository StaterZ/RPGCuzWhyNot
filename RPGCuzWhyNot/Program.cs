using System;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static int Main() {
			if (!LoadContent())
				return 1;

			//construct player
			player = new Player {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				race = new Human {
					gender = Humanoid.Gender.Male
				}
			};

			//add start items to player
			player.Inventory.MoveItem(DataLoader.CreateItem("blue_potion"));
			player.Inventory.MoveItem(DataLoader.CreateItem("backpack"));

			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				player.Handle(commandText);
			}
		}

		private static bool LoadContent() { //Load content
			Terminal.WriteLineWithoutDelay("{fg:Yellow}(Loading Content...)");
			var errorLevel = DataLoader.LoadGameData();
			if (errorLevel == DataLoader.ErrorLevel.Error)
				return false;
			Terminal.WriteLineWithoutDelay("{fg:Green}(Done!)");

			if (errorLevel == DataLoader.ErrorLevel.Success) {
				ConsoleUtils.Sleep(100);
				Terminal.Clear();
			}

			return true;
		}
	}
}