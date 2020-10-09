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
					if (combatant.health.IsAlive) {
						combatant.DoTurn(this);
					}
					Terminal.WriteLine();

					if (!combatants.Any(c => c != Program.player && c.health.IsAlive && c.WantsToHarm(Program.player))) { //slightly questionable check but it'll work for now...
						EndCombat();
					}

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