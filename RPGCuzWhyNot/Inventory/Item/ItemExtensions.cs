namespace RPGCuzWhyNot.Inventory.Item {
	public static class ItemExtensions {
		public static bool IsInsideItemWithInventory(this IItem item) => IsInsideItemWithInventory(item, out _);

		public static bool IsInsideItemWithInventory(this IItem item, out IItemWithInventory parent) {
			if (item.ContainedInventory.Owner is IItemWithInventory owner) {
				parent = owner;
				return true;
			}
			parent = default;
			return false;
		}
	}
}
