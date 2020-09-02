using System;
using System.Collections;
using System.Collections.Generic;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public abstract class InventoryBase<I, O> : IInventory, IList<I> where I : IItem where O : IHasInventory {
		protected List<I> items = new List<I>();
		public O Owner { get; }
		IHasInventory IInventory.Owner => Owner;
		protected InventoryBase(O owner) => Owner = owner;

		protected abstract bool CheckMove(I item, bool silent);

		public bool MoveItem(I item, bool silent = false) {
			if (CheckMove(item, silent)) {
				IInventory inv = item.ContainedInventory;
				if (inv != null && !inv.RemoveItem(item))
					return false;
				items.Add(item);
				item.ContainedInventory = this;
				return true;
			}
			return false;
		}

		public bool ContainsCallname(string callName, out I item) {
			foreach (I i in items) {
				if (i.CallName == callName) {
					item = i;
					return true;
				}
			}
			item = default;
			return false;
		}

		bool IInventory.ContainsCallName(string callName, out IItem item) {
			foreach (IItem i in items) {
				if (i.CallName == callName) {
					item = i;
					return true;
				}
			}
			item = default;
			return false;
		}

		void ICollection<I>.Add(I item) {
			if (!MoveItem(item, false))
				throw new InvalidOperationException();
		}

		bool IInventory.RemoveItem(IItem item) {
			if (!(item is I i))
				return false;
			return Remove(i);
		}

		public int Count => ((ICollection<I>)items).Count;
		public I this[int index] { get => ((IList<I>)items)[index]; set => ((IList<I>)items)[index] = value; }
		public int IndexOf(I item) => ((IList<I>)items).IndexOf(item);
		public void Insert(int index, I item) => ((IList<I>)items).Insert(index, item);
		public void RemoveAt(int index) => ((IList<I>)items).RemoveAt(index);
		public void Clear() => ((ICollection<I>)items).Clear();
		public bool Contains(I item) => ((ICollection<I>)items).Contains(item);
		public void CopyTo(I[] array, int arrayIndex) => ((ICollection<I>)items).CopyTo(array, arrayIndex);
		public bool Remove(I item) => ((ICollection<I>)items).Remove(item);
		public IEnumerator<I> GetEnumerator() => ((IEnumerable<I>)items).GetEnumerator();
		bool ICollection<I>.IsReadOnly => false;
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
	}
}