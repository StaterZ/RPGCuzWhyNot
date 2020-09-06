using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public interface IInventory {
		IHasInventory Owner { get; }
		bool Remove(IItem item);
		bool ContainsCallName<T>(string callName, out T item);
	}
}

