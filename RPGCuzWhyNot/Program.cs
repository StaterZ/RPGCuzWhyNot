using System;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static readonly char[] commandArgumentSeparators = new char[] { ' ' };

		private static void Main() {
			//construct world
			Location village = new Location(
				"village",
				"The Village",
				"A small village stands before you.",
				"The smoke of a little village can be seen in the distance."
			);
			village.AddItem(new WieldableWearableItem(
				"Rusty Bucket",
				"bucket",
				"A rusty old bucket",
				"There is a rusty old bucket on the ground."
			) { MeleeDamage = 3, HandsRequired = 1 });

			Location dragonNest = new Location(
				"nest",
				"The Elder Dragons Nest",
				"A large lair sprawls out around you. The deep ominous sound of the dragon breathing can be heard further in.",
				"In the horizon a dragons nest can be seen"
			);
			Location stoneFormation = new Location(
				"formation",
				"The Arcane Construct",
				"A great many stones sprawl out across the terrain.\nThey seem sprinkled around randomly across the ground.\nThe stones are full of intricately carved lines and symbols. They seem to emmit a soft purple glow.",
				"A weird configuration of stones can be seen"
			);
			Location smithy = new Location(
				"smithy",
				"Orhibalds Smithy",
				"You walk in to the smithy. The heat of the great forge and the sweat of the dirty smith purge the air.",
				"A smithy can be seen in the city"
			);
			Location smith = new Location(
				"orhibald",
				"Orhibald (Smith)",
				"You walk up to the smith. He turns around to look at you.",
				"A smith can be seen a the forge"
			);
			Location smithyCustomer = new Location(
				"customer",
				"Unknown Smithy Customer",
				"You walk up to the customer in line. She glares angrily as you approach...",
				"A customer casually stands leaning against a pillar"
			);

			village.AddPathTo(dragonNest);
			village.AddPathTo(stoneFormation);
			stoneFormation.AddPathTo(dragonNest);
			village.AddPathTo(smithy);
			smithy.AddPathTo(smith);
			smithy.AddPathTo(smithyCustomer);

			//construct player
			player = new Player {
				Name = "Bengt",
				location = village,
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