using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Things.Characters;

namespace RPGCuzWhyNot.Things.Item {
	public class TurnAction {
		public readonly IPerformableAction action;
		public readonly Character performer;
		public readonly Character target;

		public TurnAction(IPerformableAction action, Character performer, Character target = null) {
			this.action = action;
			this.performer = performer;
			this.target = target;
		}

		public void Execute() {
			action.Execute(this);
		}
	}
}