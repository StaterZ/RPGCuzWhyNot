using System;
using RPGCuzWhyNot.Data;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.Races.Humanoids;
using RPGCuzWhyNot.NPCs;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static void Main() {
			DataLoader.LoadGameData();

			Location smithy = DataLoader.GetLocation("smithy");
			smithy.AddNPC(new Orchibald(), "A smith can be seen by a large forge", "You walk up to the smith. He turns around to look at you.");
			smithy.AddNPC(new SmithyCustomer(), "A customer casually stands leaning against a pillar.", "You walk up to the customer. She glares angrily as you approach...");

			//construct player
			player = new Player {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				race = new Human {
					gender = Humanoid.Gender.Male
				}
			};

			player.Inventory.MoveItem(DataLoader.CreateItem("blue potion"));

			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Console.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Console.WriteLine();
				player.ReactToCommand(commandText);
			}
		}
	}
}