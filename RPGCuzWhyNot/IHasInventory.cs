

namespace RPGCuzWhyNot {
	public interface IHasInventory : IThing {
	}

	public interface IHaveItems : IHasInventory {
		ItemInventory Inventory { get; }
	}

	public interface ICanWear : IHasInventory {
		WearablesInventory Wearing { get; }
	}

	//public interface ICanWield : IHasInventory {
	//	WieldingInventory Wielding { get; }
	//}
}

