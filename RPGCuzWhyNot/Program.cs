using System;
using System.Collections.Generic;
using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.NPCs;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Commands;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Characters.Enemies;
using RPGCuzWhyNot.Things.Characters.NPCs;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Things.Item;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static void Main() {
			//Load content
			Terminal.WriteLineDirect("{fg:Yellow}(Loading Content...)");
			if (!DataLoader.LoadGameData()) {
				Environment.Exit(1);
			}
			Terminal.WriteLineDirect("{fg:Green}(Done!)");
			ConsoleUtils.Sleep(100);
			Terminal.Clear();

			//add npcs to smithy
			Location smithy = DataLoader.GetLocation("village_smithy");
			smithy.AddNPC(new Orchibald(), "A smith can be seen by a large forge", "You walk up to the smith. He turns around to look at you.");
			smithy.AddNPC(new SmithyCustomer(), "A customer casually stands leaning against a pillar.", "You walk up to the customer. She glares angrily as you approach...");

			//construct player
			player = new Player(new Human(Humanoid.Gender.Male)) {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				stats = new Stats(10, 10, 10, 10)
			};

			//add start items to player
			player.Inventory.MoveItem(DataLoader.CreateItem("blue_potion"));
			player.Inventory.MoveItem(DataLoader.CreateItem("backpack"));


			TestMenu();



			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				player.Handle(commandText);
			}
		}

		private static void TestMenu() {
			Stack<MenuState> menuStack = new Stack<MenuState>();
			void PushMenu(Menu menu) {
				menuStack.Push(new MenuState(menu, null));
			}

			Menu greatswordMenu = new Menu(
				new MenuItem("Light Attack", null),
				new MenuItem("Heavy Attack", null),
				new MenuItem("Throw Sword", null)
			);
			Menu staffMenu = new Menu(
				new MenuItem("Channel", null),
				new MenuItem("Fireball", null),
				new MenuItem("Fine-Point Void", null)
			);
			Menu attackMenu = new Menu(
				new MenuItem("Greatsword", () => PushMenu(greatswordMenu)),
				new MenuItem("Staff", () => PushMenu(staffMenu))
			);
			Menu rootMenu = new Menu(
				new MenuItem("{fg:Red}(Attack)", () => PushMenu(attackMenu)),
				new MenuItem("{fg:Green}(Items)", null),
				new MenuItem("{fg:Blue}(Potions)", null),
				new MenuItem("{fg:Yellow}(Armor)", null)
			);

			PushMenu(rootMenu);

			while (true) {
				MenuState menuState = menuStack.Peek();
				menuState.Draw();

				//get input
				ConsoleKeyInfo keyPress = Console.ReadKey(true);

				//move cursor
				switch (keyPress.Key) {
					case ConsoleKey.UpArrow:
						if (menuState.index.HasValue) {
							menuState.index--;
							menuState.index = ExtraMath.Mod(menuState.index.Value, menuState.menu.items.Count);
						} else {
							menuState.index = 1;
						}
						break;

					case ConsoleKey.DownArrow:
						if (menuState.index.HasValue) {
							menuState.index++;
							menuState.index = ExtraMath.Mod(menuState.index.Value, menuState.menu.items.Count);
						} else {
							menuState.index = 1;
						}
						break;

					case ConsoleKey.LeftArrow:
						menuStack.Pop();
						break;

					case ConsoleKey.RightArrow:
					case ConsoleKey.Enter:
						menuState.index ??= 0;
						menuState.menu.items[menuState.index.Value].onSelect();
						break;
				}
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