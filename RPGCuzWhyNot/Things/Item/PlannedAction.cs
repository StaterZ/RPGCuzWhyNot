using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Things.Characters;
using System;

namespace RPGCuzWhyNot.Inventory.Item {
	public class PlannedAction {
		public readonly IPlannableAction action;
		public readonly Character performer;
		public readonly Character target;

		public PlannedAction(IPlannableAction action, Character performer, Character target = null) {
			this.action = action;
			this.performer = performer;
			this.target = target;
		}

		public void Execute() {
			action.Execute(this);
		}
	}
}