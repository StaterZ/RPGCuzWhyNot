using System;
using RPGCuzWhyNot.Data;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.Races.Humanoids;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static readonly char[] commandArgumentSeparators = { ' ' };

		private static void Main() {
			ConsoleUtils.colorScopes.Add(new ConsoleUtils.ColorScope('[', ']', true, true, ConsoleColor.Magenta));

			DataLoader.LoadGameData();

			//construct player
			player = new Player {
				Name = "Bengt",
				location = DataLoader.Locations["village"],
				race = new Human {
					gender = Humanoid.Gender.Male
				}
			};

			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Console.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				string[] segments = commandText.Split(commandArgumentSeparators, StringSplitOptions.RemoveEmptyEntries);
				Console.WriteLine();
				player.ReactToCommand(segments);
			}
		}
	}
}