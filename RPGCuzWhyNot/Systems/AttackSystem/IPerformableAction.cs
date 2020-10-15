using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public interface IPerformableAction {
		string Name { get; }
		Requirements Requirements { get; }

		bool CanAfford(TurnAction turnAction);
		void Execute(TurnAction turnAction);
	}
}