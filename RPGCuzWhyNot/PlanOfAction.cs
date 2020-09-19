using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot {
	public class PlanOfAction {
		public readonly List<ItemAction> actions = new List<ItemAction>();
		public readonly Stats budget;

		public PlanOfAction(Stats budget) {
			this.budget = budget;
		}

		public Stats BudgetLeft => plannedActions.Aggregate(budget, (acc, action) => acc - action.Requirements.Stats);
		public bool IsInBudget => BudgetLeft.Speed >= 0 && BudgetLeft.Strength >= 0 && BudgetLeft.Accuracy >= 0 && BudgetLeft.Fortitude >= 0;
	}
}