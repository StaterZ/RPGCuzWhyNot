using System;
using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public class Fight {
		public List<Character> combatants;
		private bool isInCombat;

		public Fight() {
			combatants = new List<Character>();
		}

		public Fight(params Character[] combatants) {
			this.combatants = combatants.ToList();
		}

		private void DisplayCombatantStatuses() {
			Terminal.WriteLine("{fg:Cyan}(Status)");
			foreach (Character combatant in combatants) {
				ConsoleColor healthColor;
				if (combatant.health.Percent < 0.25) {
					healthColor = ConsoleColor.Red;
				} else if (combatant.health.Percent < 0.5) {
					healthColor = ConsoleColor.Yellow;
				} else {
					healthColor = ConsoleColor.Green;
				}
				Terminal.WriteLine($"{combatant.Name}: {{fg:{healthColor}}}({combatant.health.CurrentHealth}/{combatant.health.maxHealth})");
			}
			Terminal.WriteLine();
		}

		public void BeginCombat() {
			Terminal.WriteLine($"{{fg:Cyan}}(Combat with {Stringification.StringifyArray("[", ", ", "]", combatants.Select(combatant => combatant.Name).ToArray())} has begun!)");
			Terminal.WriteLine();

			int drawPos = Terminal.CursorY;
			isInCombat = true;
			while (isInCombat) {
				foreach (Character combatant in combatants) {
					while (Terminal.CursorY > drawPos) {
						Terminal.CursorY--;
						Terminal.ClearLine();
						Terminal.CursorY--;
					}

					DisplayCombatantStatuses();

					if (!combatants.Any(c => c != Program.player && c.health.IsAlive && c.WantsToHarm(Program.player))) { //slightly questionable check but it'll work for now...
						EndCombat();
					}

					if (!isInCombat) {
						break;
					}

					if (!combatant.health.IsAlive) continue;

					Terminal.WriteLine($"{{fg:Cyan}}({combatant.Name}s Turn)");
					combatant.DoTurn(this);
					if (combatant != Program.player) {
						ConsoleUtils.WaitForPlayer();
					}
					Terminal.WriteLine();
				}
			}

			Terminal.WriteLine("{fg:Cyan}(Combat has ended!)");
			Terminal.WriteLine();
		}

		public void EndCombat() {
			isInCombat = false;
		}
	}
}