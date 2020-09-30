using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Item;
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

		public void BeginCombat() {
			Terminal.WriteLine($"{{fg:Cyan}}(Combat with {Stringification.StringifyArray("[", ", ", "]", combatants.Select(combatant => combatant.Name).ToArray())} has begun!)");
			Terminal.WriteLine();

			isInCombat = true;
			while (isInCombat) {
				foreach (Character combatant in combatants) {
					Terminal.WriteLine($"{{fg:Cyan}}({combatant.Name}s Turn)");
					combatant.DoTurn(this);
					Terminal.WriteLine();

					if (!isInCombat) {
						break;
					}
				}
			}

			Terminal.WriteLine($"{{fg:Cyan}}(Combat has ended!)");
			Terminal.WriteLine();
		}

		public void EndCombat() {
			isInCombat = false;
		}
	}
}