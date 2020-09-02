using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public interface IInventory {
		IHasInventory Owner { get; }
		bool RemoveItem(IItem item);
		bool ContainsCallName(string callName, out IItem item);
	}
}

