using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Commands;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Systems.Inventory;
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
				Terminal.WriteLine($"{ctx.inflictor} hit you for {ctx.Delta} damage");
			};
			health.OnDeath += ctx => {
				Terminal.WriteLine($"{ctx.inflictor} killed you!");
			};

			commands = new PlayerCommands(this);
			commands.LoadCommands();
		}

		public override PlanOfAction PlanTurn(Fight fight) {
			const string overBudgetMessage = "{fg:Red}(You're over budget, undo the last action and try again.)";

			PlanOfAction planOfAction = new PlanOfAction(stats);

			void AddActionToPlan(PlannedAction plannedAction) {
				if (planOfAction.IsOverBudget) {
					Terminal.WriteLine(overBudgetMessage);
					return;
				}

				planOfAction.plannedActions.Add(plannedAction);
				Terminal.WriteLine($"Added action [{plannedAction.action.ListingName}] to plan.");
			}
			void RemoveActionFromPlan(PlannedAction plannedAction) {
				planOfAction.plannedActions.Remove(plannedAction);
				Terminal.WriteLine($"Removed action [{plannedAction.action.ListingName}] from plan.");
			}

			//planning phace
			bool isDonePlanningTurn = false;
			Command confirm = new Command(new[] { "confirm", "done", "apply", "execute" }, "Confirm your actions and procced to the next turn.", args => {
				if (planOfAction.IsOverBudget) {
					Terminal.WriteLine(overBudgetMessage);
					return;
				}

				isDonePlanningTurn = true;
			});
			Command undo = new Command(new[] { "undo", "revert" }, "Remove the last move you planned to do from the plan of action.", args => {
				if (planOfAction.plannedActions.Count > 0) {
					PlannedAction last = planOfAction.plannedActions.Last();
					RemoveActionFromPlan(last);
				} else {
					Terminal.WriteLine("You've got no plan of action. There's nothing to regret...");
				}
			});
			Command run = new Command(new[] { "run", "run away" }, "Run away from the fight.", args => {
				AddActionToPlan(new PlannedAction(new RunFromFightAction(fight), this));
			});
			Command plan = new Command(new[] { "ls", "list", "plan" }, "Remove the last move you planned to do from the plan of action.", args => {
				if (planOfAction.plannedActions.Count > 0) {
					Terminal.WriteLine("{fg:Cyan}(Plan of action:)");
					foreach (IPlannableAction plannedAction in planOfAction.plannedActions) {
						Terminal.WriteLine($" - {plannedAction.ListingName}");
					}
				} else {
					Terminal.WriteLine("You've got no plan!");
				}
			});

			while (!isDonePlanningTurn) {
				//find valid commands
				CommandHandler handler = new CommandHandler();
				handler.AddCommand(confirm);
				handler.AddCommand(undo);
				handler.AddCommand(plan);
				handler.AddCommand(run);
				handler.AddCommand(new Command(new[] { "help", "commands" }, "Show this list", args => {
					Terminal.WriteLine("Commands:");
					handler.DisplayHelp();
				}));

				foreach (IWieldable wieldable in Wielding) {
					handler.AddCommand(new Command(new[] { wieldable.CallName }, "Do something with this item.", args => {
						if (args.FirstArgument == "") {
							Terminal.WriteLine("Do what with it?");
							return;
						}

						CommandHandler itemHandler = new CommandHandler();

						foreach (ItemAction itemAction in wieldable.ItemActions) {
							itemHandler.AddCommand(new Command(itemAction.CallNames, itemAction.Description, itemArgs => {
								//todo: make selection for target when menu has been merged inot branch
								Character target = fight.combatants.First(combatant => combatant != this);
								AddActionToPlan(new PlannedAction(itemAction, this, target));
							}));
						}

						itemHandler.AddCommand(new Command(new[] { "help" }, "Get help for this item.", args => {
							Terminal.WriteLine("Actions:");
							if (wieldable.ItemActions.Any()) {
								itemHandler.DisplayHelp();
							} else {
								Terminal.WriteLine("There's no actions for this item.");
							}
						}));

						if (!itemHandler.TryHandle(args.FirstArgument)) {
							Terminal.WriteLine("No such action exists for this item.");
						}
					}));
				}

				//get next command
				Terminal.WriteLine();
				Terminal.Write($"Points Left: {planOfAction.BudgetLeft.Listing}");
				if (planOfAction.IsOverBudget) {
					Terminal.Write(" {fg:White;bg:Red}([Over Budget])");
				}
				Terminal.WriteLine();

				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				if (!handler.TryHandle(commandText)) {
					Terminal.WriteLine("I don't understand.");
				}
			}
			Terminal.WriteLine("Now Executeing actions...");

			return planOfAction;
		}

		public void Handle(string message) {
			commands.Handle(message);
		}
	}
}

