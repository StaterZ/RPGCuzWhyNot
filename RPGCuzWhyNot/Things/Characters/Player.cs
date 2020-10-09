using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.CommandSystem;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Systems.Inventory;
using RPGCuzWhyNot.Systems.MenuSystem;
using RPGCuzWhyNot.Things.Characters.Races;
using RPGCuzWhyNot.Things.Item;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Characters {
	public class Player : Character {
		private readonly PlayerCommands commands;

		public Player(Race race) : base(race) {
			//init health
			health = new Health(100);
			health.OnDamage += ctx => {
				Terminal.WriteLine($"{ctx.inflictor} hit you for {ctx.Delta} damage.");
			};
			health.OnDeath += ctx => {
				Terminal.WriteLine($"{ctx.inflictor} killed you!");
			};

			commands = new PlayerCommands();
			commands.LoadCommands();
		}

		public override void DoTurn(Fight fight) {
			const string performFailMessage = "{fg:Red}(You can't afford this action right now.)";

			bool isDonePlanningTurn = false;
			TurnActions turnActions = new TurnActions(stats);

			Menu CreateCombatantSelectMenu(string menuName, IEnumerable<Character> combatants, Action<Character> combatantSelectCallback) {
				Menu menu = new Menu(menuName);

				foreach (Character combatant in combatants) {
					menu.items.Add(new MenuItem(combatant.Name, $"Do action on {combatant.Name}", ctx => {
						combatantSelectCallback(combatant);
						ctx.ExitMenu();
					}));
				}

				return menu;
			}

			void InsertItemActions(Menu menu, IEnumerable<ItemAction> itemActions) {
				foreach (ItemAction itemAction in itemActions) {
					menu.items.Add(new MenuItem(itemAction.Name, itemAction.Description, ctx => {
						Character target = null;

						if (itemAction.HasTarget) {
							ctx.EnterMenu(CreateCombatantSelectMenu(
								itemAction.Name,
								fight.combatants.Where(combatant => combatant != this),
								combatant => target = combatant)
							);

							//if target is null then we didn't select any combatant in CreateCombatantSelectMenu and the only way to do that is to back out of the menu
							//if we backed out, return so to not execute the action with null
							if (target == null) {
								return;
							}
						}


						if (!turnActions.TryPerform(new TurnAction(itemAction, this, target))) {
							Terminal.WriteLine(performFailMessage);
						}
						ConsoleUtils.WaitForPlayer();
						ctx.ExitEntireMenuStack();
					}));
				}
			}

			while (!isDonePlanningTurn) {
				Menu attack = new Menu("{fg:Red}(Attack)");
				foreach (IWieldable wieldable in Wielding) {
					Menu menu = new Menu(wieldable.Name);
					attack.items.Add(new SubMenu(menu, "Do something with this item. (Temp Desc)"));

					InsertItemActions(menu, wieldable.ItemActions);
				}

				Menu allItems = new Menu("{fg:White}(All)");
				Menu wearableItems = new Menu("{fg:Yellow}(Wearables)");
				Menu wieldableItems = new Menu("{fg:Yellow}(Wieldables)");
				Menu inventoriesItems = new Menu("{fg:DarkYellow}(Inventories)");
				Menu consumableItems = new Menu("{fg:Blue}(Consumables)");
				foreach (IItem item in Inventory) {
					Menu menu = new Menu(item.Name);

					allItems.items.Add(new SubMenu(menu, "Do something with this item. (Temp Desc)"));
					if (item is IWieldable) {
						wieldableItems.items.Add(new SubMenu(menu, "Do something with this item. (Temp Desc)"));
					}
					if (item is IWearable) {
						wearableItems.items.Add(new SubMenu(menu, "Do something with this item. (Temp Desc)"));
					}
					if (item is ItemWithInventory) {
						inventoriesItems.items.Add(new SubMenu(menu, "Do something with this item. (Temp Desc)"));
					}
					if (item.ItemActions.Any(action => action.Effects.ConsumeSelf)) {
						consumableItems.items.Add(new SubMenu(menu, "Do something with this item. (Temp Desc)"));
					}

					InsertItemActions(menu, item.ItemActions);
				}

				Menu items = new Menu("{fg:Green}(Items)",
					new SubMenu(allItems, "List everything"),
					new SubMenu(wearableItems, "List wearables"),
					new SubMenu(wieldableItems, "List wieldables"),
					new SubMenu(consumableItems, "List consumables")
				);

				Menu equipment = new Menu("{fg:Yellow}(Equipment)");
				foreach (IWearable wearable in Wearing) {
					Menu menu = new Menu(wearable.Name);
					equipment.items.Add(new SubMenu(menu, "Do something with this item. (Temp Desc)"));

					InsertItemActions(menu, wearable.ItemActions);
				}

				Menu root = new Menu("Root",
					new SubMenu(attack, "Attack something"),
					new SubMenu(items, "Use an item"),
					new SubMenu(equipment, "Manage equippment"),
					new MenuItem("Flee", "Run away from the fight.", ctx => {
						//fight.combatants.Remove(this);
						fight.EndCombat();

						isDonePlanningTurn = true;
						ctx.ExitEntireMenuStack();
					}),
					new MenuItem("EndTurn", "Confirm your actions and proceed to the next turn.", ctx => {
						isDonePlanningTurn = true;
						ctx.ExitEntireMenuStack();
					})
				);

				Terminal.WriteLineWithoutDelay($"Points Left: {turnActions.BudgetLeft.Listing}");
				root.Enter();
				Terminal.CursorPosition += Vec2.Up;
				Terminal.ClearLine();
				Terminal.CursorPosition += Vec2.Up;
			}
		}

		public void Handle(string message) {
			commands.Handle(message);
		}

		public override bool WantsToHarm(Character character) {
			return true;
		}
	}
}

