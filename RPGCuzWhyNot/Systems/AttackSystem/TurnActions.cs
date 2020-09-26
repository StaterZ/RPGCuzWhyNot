using RPGCuzWhyNot.Things.Item;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public class TurnActions {
		public readonly Stats budget;
		private readonly List<TurnAction> actions = new List<TurnAction>();
		public ReadOnlyCollection<TurnAction> Actions { get; }

		public TurnActions(Stats budget) {
			this.budget = budget;
			Actions = actions.AsReadOnly();
		}

		public Stats BudgetLeft => actions.Aggregate(budget, (acc, plannedAction) => acc - plannedAction.action.Requirements.Stats);

		public bool CanAfford(TurnAction action) {
			Stats budgetLeftAfterAction = BudgetLeft - action.action.Requirements.Stats;

			return
				budgetLeftAfterAction.Speed >= 0 &&
				budgetLeftAfterAction.Strength >= 0 &&
				budgetLeftAfterAction.Accuracy >= 0 &&
				budgetLeftAfterAction.Fortitude >= 0;
		}

		public bool TryPerform(TurnAction action) {
			if (!CanAfford(action)) return false;

			actions.Add(action);
			action.Execute();

			return true;
		}
	}
}