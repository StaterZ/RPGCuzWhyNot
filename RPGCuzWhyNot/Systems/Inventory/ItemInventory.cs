using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Inventory {
	public class ItemInventory : InventoryBase<IItem, IHasInventory> {
		public ItemInventory(IHasInventory owner) : base(owner) { }

		protected override bool CheckMove(IItem item, bool silent) => true;

		public IItem GetItemById(string id)
		{
			foreach (IItem item in items) {
				if (item.Prototype.Id == id) {
					return item;
				}
			}

			return null;
		}
	}
}

