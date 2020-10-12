using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RPGCuzWhyNot.Systems.Inventory;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public class Turn {
		public readonly Stats budget;

		public Turn(Stats budget) {
			this.budget = budget;
		}

		public Stats BudgetLeft { get; private set; }

		private bool CanAffordStats(TurnAction turnAction) {
			Stats budgetLeftAfterAction = BudgetLeft - turnAction.action.Requirements.Stats;

			return
				budgetLeftAfterAction.Speed >= 0 &&
				budgetLeftAfterAction.Strength >= 0 &&
				budgetLeftAfterAction.Accuracy >= 0 &&
				budgetLeftAfterAction.Fortitude >= 0;
		}

		public bool TryPerform(TurnAction turnAction) {
			if (!CanAffordStats(turnAction)) return false;
			if (!turnAction.CanAfford()) return false;

			BudgetLeft -= turnAction.action.Requirements.Stats;

			turnAction.Execute();

			return true;
		}
	}
}