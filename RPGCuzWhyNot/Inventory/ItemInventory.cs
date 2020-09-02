using System.Collections;
using System.Collections.Generic;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public class ItemInventory : IInventory, IList<IItem> {
		private readonly List<IItem> items = new List<IItem>();
		public int Count => items.Count;
		public IHasItemInventory Owner { get; }
		public IItem this[int index] { get => ((IList<IItem>)items)[index]; set => ((IList<IItem>)items)[index] = value; }
		public ItemInventory(IHasItemInventory owner) => Owner = owner;
		IHasInventory IInventory.Owner => Owner;

		public bool MoveItem(IItem item) {
			IInventory inv = item.ContainedInventory;
			if (inv != null && !inv.RemoveItem(item))
				return false;
			items.Add(item);
			item.ContainedInventory = this;
			return true;
		}

		public bool ContainsCallName(string callName, out IItem item) {
			foreach (IItem i in items) {
				if (i.CallName == callName) {
					item = i;
					return true;
				}
			}
			item = default;
			return false;
		}

		bool IInventory.RemoveItem(IItem item) => Remove(item);
		void ICollection<IItem>.Add(IItem item) => MoveItem(item);
		bool ICollection<IItem>.IsReadOnly => false;
		public int IndexOf(IItem item) => ((IList<IItem>)items).IndexOf(item);
		public void Insert(int index, IItem item) => ((IList<IItem>)items).Insert(index, item);
		public void RemoveAt(int index) => ((IList<IItem>)items).RemoveAt(index);
		public void Clear() => ((ICollection<IItem>)items).Clear();
		public bool Contains(IItem item) => ((ICollection<IItem>)items).Contains(item);
		public bool Remove(IItem item) => ((ICollection<IItem>)items).Remove(item);
		void ICollection<IItem>.CopyTo(IItem[] array, int arrayIndex) => ((ICollection<IItem>)items).CopyTo(array, arrayIndex);
		public IEnumerator<IItem> GetEnumerator() => ((IEnumerable<IItem>)items).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
	}
}