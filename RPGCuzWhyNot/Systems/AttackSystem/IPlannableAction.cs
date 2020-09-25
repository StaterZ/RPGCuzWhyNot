using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public interface IPlannableAction {
		string Name { get; }
		string ListingName { get; }
		string ExecuteDescription { get; }
		bool HasTarget { get; }
		Requirements Requirements { get; }

		void Execute(PlannedAction ctx);
	}
}