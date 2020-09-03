using System;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.Races.Humanoids;
using RPGCuzWhyNot.NPCs;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static readonly char[] commandArgumentSeparators = { ' ' };

		private static void Main() {
			ConsoleUtils.colorScopes.Add(new ConsoleUtils.ColorScope('[', ']', true, true, ConsoleColor.Magenta));
			ConsoleUtils.colorScopes.Add(new ConsoleUtils.ColorScope('<', '>', true, true, ConsoleColor.Green));

			//construct world
			Location village = new Location(
				"village",
				"The Village",
				"A small village stands before you."
			);
			village.AddItem(new WieldableWearableItem(
				"Rusty Bucket",
				"bucket",
				"A rusty old bucket",
				"There is a rusty old bucket on the ground."
			) {
				MeleeDamage = 3,
				HandsRequired = 1,

				Defense = 1,
				CoveredParts = BodyParts.Head,
				CoveredLayers = WearableLayers.Outer,
			});

			Location dragonNest = new Location(
				"nest",
				"The Elder Dragons Nest",
				"A large lair sprawls out around you. The deep ominous sound of the dragon breathing can be heard further in."
			);
			Location stoneFormation = new Location(
				"formation",
				"The Arcane Construct",
				"A great many stones sprawl out across the terrain.\nThey seem sprinkled around randomly across the ground.\nThe stones are full of intricately carved lines and symbols. They seem to emmit a soft purple glow."
			);
			Location smithy = new Location(
				"smithy",
				"Orhibalds Smithy",
				"You walk in to the smithy. The heat of the great forge and the sweat of the dirty smith purge the air."
			);
			smithy.AddNPC(new Orchibald(), "A smith can be seen by a large forge", "You walk up to the smith. He turns around to look at you.");
			smithy.AddNPC(new SmithyCustomer(), "A customer casually stands leaning against a pillar.", "You walk up to the customer. She glares angrily as you approach...");


			village.AddPathTo(dragonNest, "In the horizon a dragons nest can be seen atop a great mountain.");
			dragonNest.AddPathTo(village, "The smoke of a little village can be seen in the distance.");

			village.AddPathTo(stoneFormation, "A weird configuration of stones can be seen");
			stoneFormation.AddPathTo(village, "A village can be seen a few hundred meters away");

			stoneFormation.AddPathTo(dragonNest, "In the horizon a dragons nest can be seen atop a great mountain.");
			dragonNest.AddPathTo(stoneFormation, "A weird configuration of stones can be seen far bellow the vgreat mountain");

			village.AddPathTo(smithy, "A smithy can be seen in the city.");
			smithy.AddPathTo(village, "A door out of the smithy can be seen behind you.");

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