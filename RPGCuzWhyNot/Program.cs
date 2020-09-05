using System;
using RPGCuzWhyNot.Data;
using RPGCuzWhyNot.Enemies;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.Races.Humanoids;
using RPGCuzWhyNot.NPCs;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;
		public static PlayerCommands commands;

		private static void Main() {
			DataLoader.LoadGameData();

			Location smithy = DataLoader.GetLocation("village_smithy");
			smithy.AddNPC(new Orchibald(), "A smith can be seen by a large forge", "You walk up to the smith. He turns around to look at you.");
			smithy.AddNPC(new SmithyCustomer(), "A customer casually stands leaning against a pillar.", "You walk up to the customer. She glares angrily as you approach...");
			
			smithy.AddItem(new WieldableItem("Greatsword", "sword", "A shineing sword of the finest metal", "There's a sword on the ground; shining brilliantly in the sunlight") {
				HandsRequired = 2,
				ItemActions = new[] {
					new ItemAction(
						new[] { "light attack", "light" }, 
						"Light Attack", 
						"Swing the sword about and hope to hit something...", 
						new Requirements(new Stats(0, 2, 0, 0)), 
						new Effects(new Stats(0, 0, 0, 0), 10, 0, 0, false)
					),
					new ItemAction(
						new[] { "heavy attack", "heavy" }, 
						"Heavy Attack", 
						"Swing the greatsword with all your might", 
						new Requirements(new Stats(0, 5, 0, 0)), 
						new Effects(new Stats(0, 0, 0, 0), 20, 0, 0.2f, false)
					),
					new ItemAction(
						new[] { "throw" }, 
						"Throw Sword", 
						"Throw the sword at an enemy with great sword", 
						new Requirements(new Stats(0, 10, 5, 0)), 
						new Effects(new Stats(0, 0, 0, 0), 0, 50, 1, true)
					)
				}
			});


			//construct player
			player = new Player(new Human(Humanoid.Gender.Male)) {
				Name = "Bengt",
				location = DataLoader.GetLocation("village")
			};

			commands = new PlayerCommands(player);
			commands.LoadCommands();

			player.Inventory.MoveItem(DataLoader.CreateItem("blue_potion"));

			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				commands.Handle(commandText);
			}
		}



		public static void EnterCombat(params Character[] opponents) {
			while (true) {
				//Players Turn
				Terminal.WriteLine($"{player.Name}s Turn (You)");
				player.PlanTurn(opponents);

				//Opponents Turn
				foreach (Character opponent in opponents) {
					Terminal.WriteLine($"{opponent.Name}s Turn");
					opponent.PlanTurn(player);
				}
			}
		}
	}
}