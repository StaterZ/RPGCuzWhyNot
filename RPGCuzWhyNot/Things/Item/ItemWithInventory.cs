using System.Collections;
using System.Collections.Generic;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems.Inventory;

namespace RPGCuzWhyNot.Things.Item {
	public class ItemWithInventory : WieldableItem, IItemWithInventory {
		public ItemInventory Inventory { get; }
		public Fraction WeightFraction { get; set; } = new Fraction(1, 1);

		public ItemWithInventory(string name, string callName, string descriptionInInventory, string descriptionOnGround)
			: base(name, callName, descriptionInInventory, descriptionOnGround) => Inventory = new ItemInventory(this);

		public bool ContainsCallName(string callName, out IItem item) => Inventory.ContainsCallName(callName, out item);
		public bool MoveItem(IItem item, bool silent = false) => Inventory.MoveItem(item, silent);

		IEnumerator<IItem> IEnumerable<IItem>.GetEnumerator() => Inventory.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Inventory).GetEnumerator();
	}
}
