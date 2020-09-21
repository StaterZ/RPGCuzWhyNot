using Microsoft.VisualBasic.CompilerServices;
using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace RPGCuzWhyNot.AttackSystem {
	public class Fight {
		public List<Character> combatants;
		private bool isInCombat;

		public Fight() {
			combatants = new List<Character>();
		}

		public Fight(params Character[] combatants) {
			this.combatants = combatants.ToList();
		}

		public void BeginCombat() {
			Terminal.WriteLine($"{{fg:Cyan}}(Combat with {Stringification.StringifyArray("[", ", ", "]", combatants.Select(combatant => combatant.Name).ToArray())} has begun!)");
			isInCombat = true;
			while (isInCombat) {
				//Players Turn
				Terminal.WriteLine($"{Program.player.Name}s Turn (You)");
				PlanOfAction playersPlanOfAction = Program.player.PlanTurn(this);
				foreach (IPlannableAction action in playersPlanOfAction.plannedActions) {
					action.Execute();
				}

				//Opponents Turn
				foreach (Character combatant in combatants) {
					Terminal.WriteLine($"{combatant.Name}s Turn");
					PlanOfAction opponentsPlanOfAction = combatant.PlanTurn(this);
					foreach (IPlannableAction action in opponentsPlanOfAction.plannedActions) {
						action.Execute();
					}
				}
			}
			Terminal.WriteLine($"{{fg:Cyan}}(Combat has ended!)");
		}

		public void EndCombat() {
			isInCombat = false;
		}
	}
}