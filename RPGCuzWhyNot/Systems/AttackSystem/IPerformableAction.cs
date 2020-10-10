using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public interface IPerformableAction {
		bool HasTarget { get; }
		string Name { get; }
		string ListingName { get; }
		string ExecuteDescription { get; }
		Requirements Requirements { get; }

		void Execute(TurnAction turnAction);
	}
}