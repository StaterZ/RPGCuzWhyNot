namespace RPGCuzWhyNot {
	public interface IInventory {
		IHasInventory Owner { get; }
		bool RemoveItem(IItem item);
		bool ContainsCallName(string callName, out IItem item);
	}
}

