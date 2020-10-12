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
			Terminal.WriteLine($"{{fg:Cyan}}(Combat with {Utils.StringifyArray("[", ", ", "]", combatants.Select(combatant => combatant.Name).ToArray())} has begun!)");
			Terminal.WriteLine();

			Terminal.WriteLineWithoutDelay("{fg:Cyan}(Status)");
			int drawPos = Terminal.CursorY;

			(Character combatant, Action<HealthChangeInfo> healthStatusDisplay)[] healthStatusDisplayDatas =
				new (Character combatant, Action<HealthChangeInfo> healthStatusDisplay)[combatants.Count];

			for (int i = 0; i < combatants.Count; i++) {
				Character combatant = combatants[i];

				int myi = i;

				void UpdateCombatantStatus(HealthChangeInfo ctx) {
					int currentPos = Terminal.CursorY;
					Terminal.CursorY = drawPos + myi;

					ConsoleColor healthColor;
					if (combatant.health.Percent < 0.25) {
						healthColor = ConsoleColor.Red;
					} else if (combatant.health.Percent < 0.5) {
						healthColor = ConsoleColor.Yellow;
					} else {
						healthColor = ConsoleColor.Green;
					}

					Terminal.ClearLine();
					Terminal.CursorY--;
					Terminal.WriteLineWithoutDelay($"{combatant.Name}: {{fg:{healthColor}}}({combatant.health.CurrentHealth}/{combatant.health.maxHealth})");

					Terminal.WriteLineWithoutDelay();

					Terminal.CursorY = currentPos;
				}

				healthStatusDisplayDatas[i] = (combatant, UpdateCombatantStatus);
				combatant.health.OnChange += UpdateCombatantStatus;
			}

			foreach ((Character combatant, Action<HealthChangeInfo> healthStatusDisplay) healthStatusDisplayData in healthStatusDisplayDatas) {
				healthStatusDisplayData.healthStatusDisplay(default);
			}

			isInCombat = true;
			while (isInCombat) {
				foreach (Character combatant in combatants) {
					while (Terminal.CursorY > drawPos) {
						Terminal.CursorY--;
						Terminal.ClearLine();
						Terminal.CursorY--;
					}

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
						Utils.WaitForPlayer();
					}
					Terminal.WriteLine();
				}
			}

			foreach ((Character combatant, Action<HealthChangeInfo> healthStatusDisplay) healthStatusDisplayData in healthStatusDisplayDatas) {
				healthStatusDisplayData.combatant.health.OnChange -= healthStatusDisplayData.healthStatusDisplay;
			}

			Terminal.WriteLine("{fg:Cyan}(Combat has ended!)");
			Terminal.WriteLine();
		}

		public void EndCombat() {
			isInCombat = false;
		}
	}
}