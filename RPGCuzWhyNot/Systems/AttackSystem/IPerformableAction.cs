using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.AttackSystem {
	public interface IPerformableAction {
		string Name { get; }
		Requirements Requirements { get; }

		void Execute(TurnAction turnAction);
	}
}