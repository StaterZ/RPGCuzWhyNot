using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public class ItemInventory : InventoryBase<IItem, IHasInventory> {
		public ItemInventory(IHasInventory owner) : base(owner) { }

		protected override bool CheckMove(IItem item, bool silent) => true;
	}
}

