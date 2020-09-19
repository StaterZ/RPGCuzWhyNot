﻿using RPGCuzWhyNot.Data;
using RPGCuzWhyNot.Enemies;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.NPCs;
using RPGCuzWhyNot.Races.Humanoids;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;
		public static PlayerCommands commands;

		private static void Main() {
			DataLoader.LoadGameData();

			Location smithy = DataLoader.GetLocation("village_smithy");
			smithy.AddNPC(new Orchibald(), "A smith can be seen by a large forge", "You walk up to the smith. He turns around to look at you.");
			smithy.AddNPC(new SmithyCustomer(), "A customer casually stands leaning against a pillar.", "You walk up to the customer. She glares angrily as you approach...");
			


			//construct player
			player = new Player(new Human(Humanoid.Gender.Male)) {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				stats = new Stats(10, 10, 10, 10)
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

		private static bool isInCombat;
		public static void EnterCombat(params Character[] opponents) {
			isInCombat = true;
			while (isInCombat) {
				//Players Turn
				Terminal.WriteLine($"{player.Name}s Turn (You)");
				PlanOfAction playersPlanOfAction = player.PlanTurn(opponents);
				foreach (IPlannableAction action in playersPlanOfAction.plannedActions) {
					action.Execute();
				}

				//Opponents Turn
				foreach (Character opponent in opponents) {
					Terminal.WriteLine($"{opponent.Name}s Turn");
					PlanOfAction opponentsPlanOfAction = opponent.PlanTurn(player);
					foreach (IPlannableAction action in opponentsPlanOfAction.plannedActions) {
						action.Execute();
					}
				}
			}
		}

		public static void ExitCombat() {
			isInCombat = false;
		}
	}
}