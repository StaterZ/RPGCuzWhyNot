using System;
using System.Collections;
using System.Collections.Generic;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public abstract class InventoryBase<ItemT, OwnerT> : IInventory, IEnumerable<ItemT> where ItemT : IItem where OwnerT : IHasInventory {
		protected List<ItemT> items = new List<ItemT>();
		public OwnerT Owner { get; }
		IHasInventory IInventory.Owner => Owner;
		protected InventoryBase(OwnerT owner) => Owner = owner;

		protected abstract bool CheckMove(ItemT item, bool silent);

		private bool CheckRecursion(ItemT item, bool silent) {
			for (IItemWithInventory i = item as IItemWithInventory; i != null; i = i.ContainedInventory.Owner as IItemWithInventory) {
				if (i.Equals(Owner)) {
					if (!silent) {
						Terminal.WriteLine($"{item.Name} can't be contained inside {Owner.Name}, as it would end up inside itself.");
					}
					return false;
				}
			}
			return true;
		}

		public bool MoveItem(ItemT item, bool silent = false) {
			if (item.ContainedInventory == null || (CheckRecursion(item, silent) && CheckMove(item, silent))) {
				IInventory inv = item.ContainedInventory;
				if (inv != null && !inv.Remove(item))
					return false;
				items.Add(item);
				item.ContainedInventory = this;
				return true;
			}
			return false;
		}

		public bool ContainsCallName<T>(string callName, out T item) where T : IItem {
			foreach (ItemT i in items) {
				if (i.CallName == callName && i is T ti) {
					item = ti;
					return true;
				}
			}
			item = default;
			return false;
		}

		bool IInventory.Remove(IItem item) {
			if (item is ItemT i)
				return Remove(i);
			return false;
		}

		public bool Remove(ItemT item) {
			return items.Remove(item);
		}

		public int Count => items.Count;
		public ItemT this[int index] => items[index];

		public void Clear() => items.Clear();
		public IEnumerator<ItemT> GetEnumerator() => items.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
	}
}

