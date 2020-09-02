namespace RPGCuzWhyNot.Inventory {
	public interface IHasInventory : IThing {
	}

	public interface IHasItemInventory : IHasInventory {
		ItemInventory Inventory { get; }
	}

	public interface ICanWear : IHasInventory {
		WearablesInventory Wearing { get; }
	}

	public interface ICanWield : IHasInventory {
		WieldablesInventory Wielding { get; }
	}
}

