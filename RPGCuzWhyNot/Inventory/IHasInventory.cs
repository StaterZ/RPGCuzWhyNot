using System.Collections.Generic;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public interface IHasInventory : IThing, IEnumerable<IItem> {
		bool ContainsCallName(string callName, out IItem item);
		bool MoveItem(IItem item, bool silent = false);
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

