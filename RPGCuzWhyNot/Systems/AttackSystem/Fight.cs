using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public class Fight {
		private readonly List<Character> combatants;
		private bool isInCombat;

		public ReadOnlyCollection<Character> Combatants { get; }

		public Fight() {
			combatants = new List<Character>();
		}

		public Fight(params Character[] combatants) {
			this.combatants = combatants.ToList();
			Combatants = this.combatants.AsReadOnly();
		}

		public void BeginCombat() {
			Terminal.WriteLine($"{{fg:Cyan}}(Combat with {Utils.StringifyArray("[", ", ", "]", combatants.Select(combatant => combatant.Name).ToArray())} has begun!)\n");

			Terminal.WriteLineWithoutDelay("{fg:Cyan}(Status)");
			int drawPos = Terminal.CursorY;

			(Character combatant, Action<HealthChangeInfo> healthStatusDisplay)[] healthStatusDisplayDatas =
				new (Character, Action<HealthChangeInfo>)[combatants.Count];

			for (int i = 0; i < combatants.Count; i++) {
				Character combatant = combatants[i];
				int statusDrawPos = drawPos + i;

				void HealthStatusDisplay(HealthChangeInfo ctx) => UpdateCombatantStatus(combatant, statusDrawPos);

				healthStatusDisplayDatas[i] = (combatant, HealthStatusDisplay);
				combatant.health.OnChange += HealthStatusDisplay;
			}

			foreach ((Character _, Action<HealthChangeInfo> healthStatusDisplay) in healthStatusDisplayDatas) {
				healthStatusDisplay(default);
			}

			isInCombat = true;
			while (isInCombat) {
				DoCombat(drawPos);
			}

			foreach ((Character combatant, Action<HealthChangeInfo> healthStatusDisplay) in healthStatusDisplayDatas) {
				combatant.health.OnChange -= healthStatusDisplay;
			}

			Terminal.WriteLine("{fg:Cyan}(Combat has ended!)\n");
		}

		public void EndCombat() {
			isInCombat = false;
		}

		private static void UpdateCombatantStatus(Character combatant, int drawPos) {
			int currentPos = Terminal.CursorY;
			Terminal.CursorY = drawPos;

			ConsoleColor healthColor = ColorBasedOnHealth(combatant.health);

			Terminal.ClearLine();
			Terminal.CursorY--;
			Terminal.WriteLineWithoutDelay($"{combatant.Name}: {{fg:{healthColor}}}({combatant.health.CurrentHealth}/{combatant.health.maxHealth})\n");
			Terminal.CursorY = currentPos;
		}

		private static ConsoleColor ColorBasedOnHealth(Health health) {
			if (health.Percent < 0.25) return ConsoleColor.Red;
			if (health.Percent < 0.5) return ConsoleColor.Yellow;

			return ConsoleColor.Green;
		}

		private void DoCombat(int drawPos) {
			foreach (Character combatant in combatants) {
				while (Terminal.CursorY > drawPos) {
					Terminal.CursorY--;
					Terminal.ClearLine();
					Terminal.CursorY--;
				}

				if (!combatants.Any(c => c != Program.player && c.health.IsAlive && c.WantsToHarm(Program.player))) {
					//slightly questionable check but it'll work for now...
					EndCombat();
				}

				if (!isInCombat) break;
				if (!combatant.health.IsAlive) continue;

				Terminal.WriteLine($"{{fg:Cyan}}({combatant.Name}'s Turn)");
				combatant.DoTurn(this);
				if (combatant != Program.player) {
					Utils.WaitForPlayer();
				}

				Terminal.WriteLine();
			}
		}
	}
}