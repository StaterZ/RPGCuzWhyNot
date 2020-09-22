using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.AttackSystem {
	public class RunFromFightAction : IPlannableAction {
		private readonly Fight fight;

		public string Name => "Run Away";
		public string ListingName => Name;
		public string ExecuteDescription => "Fear strikes your body and you run off crying like a baby";
		public bool HasTarget => false;
		public Requirements Requirements { get; } = new Requirements();

		public RunFromFightAction(Fight fight) {
			this.fight = fight;
		}

		public void Execute(PlannedAction plannedAction) {
			fight.combatants.Remove(Program.player);
			fight.EndCombat();
		}
	}
}