using System.Collections;
using System.Collections.Generic;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Inventory {
	public abstract class InventoryBase<TItem, TOwner> : IInventory, IEnumerable<TItem> where TItem : IItem where TOwner : IHasInventory {
		protected List<TItem> items = new List<TItem>();
		public TOwner Owner { get; }
		IHasInventory IInventory.Owner => Owner;
		protected InventoryBase(TOwner owner) => Owner = owner;

		protected abstract bool CheckMove(TItem item, bool silent);

		private bool CheckRecursion(TItem item, bool silent) {
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

		public bool MoveItem(TItem item, bool silent = false) {
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

		public bool ContainsCallName<T>(string callName, out T item) {
			foreach (TItem i in items) {
				if (i.CallName == callName && i is T ti) {
					item = ti;
					return true;
				}
			}
			item = default;
			return false;
		}

		bool IInventory.Remove(IItem item) {
			if (item is TItem i)
				return Remove(i);
			return false;
		}

		public bool Remove(TItem item) {
			return items.Remove(item);
		}

		public bool TryGetItemById(string id, out TItem result) {
			foreach (TItem item in items) {
				if (item.Prototype.Id == id) {
					result = item;
					return true;
				}
			}

			result = default;
			return false;
		}

		public int GetItemCountById(string id) {
			int count = 0;

			foreach (TItem item in items) {
				if (item.Prototype.Id == id) {
					count++;
				}
			}

			return count;
		}

		public int Count => items.Count;
		public TItem this[int index] => items[index];

		public void Clear() => items.Clear();
		public IEnumerator<TItem> GetEnumerator() => items.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
	}
}

