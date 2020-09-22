using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.AttackSystem {
	public class PlanOfAction {
		public readonly Stats budget;
		public readonly List<PlannedAction> plannedActions = new List<PlannedAction>();

		public PlanOfAction(Stats budget) {
			this.budget = budget;
		}

		public Stats BudgetLeft => plannedActions.Aggregate(budget, (acc, plannedAction) => acc - plannedAction.action.Requirements.Stats);
		public bool IsOverBudget => BudgetLeft.Speed < 0 || BudgetLeft.Strength < 0 || BudgetLeft.Accuracy < 0 || BudgetLeft.Fortitude < 0;
	}
}