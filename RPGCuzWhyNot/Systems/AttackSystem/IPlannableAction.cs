using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.AttackSystem {
	public interface IPlannableAction {
		string Name { get; }
		string ListingName { get; }
		string ExecuteDescription { get; }
		Requirements Requirements { get; }

		void Execute(PlannedAction ctx);
	}
}