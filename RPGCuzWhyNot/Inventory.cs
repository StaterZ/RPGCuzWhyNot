using System;
using System.Collections;
using System.Collections.Generic;

namespace RPGCuzWhyNot {
	public interface IInventory {
		IHasInventory Owner { get; }
		bool RemoveItem(IItem item);
	}

	public class ItemInventory : IInventory, IList<IItem> {
		private readonly List<IItem> items = new List<IItem>();
		public int Count => items.Count;
		public IHaveItems Owner { get; }
		public IItem this[int index] { get => ((IList<IItem>)items)[index]; set => ((IList<IItem>)items)[index] = value; }
		public ItemInventory(IHaveItems owner) => Owner = owner;
		IHasInventory IInventory.Owner => Owner;

		public void MoveTo(IItem item) {
			item.ContainedInventory?.RemoveItem(item);
			items.Add(item);
			item.ContainedInventory = this;
		}

		bool IInventory.RemoveItem(IItem item) => Remove(item);
		void ICollection<IItem>.Add(IItem item) => MoveTo(item);
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

	public abstract class Inventory<I, O> : IInventory, IList<I> where I : IItem where O : IHasInventory {
		protected List<I> items = new List<I>();
		public O Owner { get; }
		IHasInventory IInventory.Owner => Owner;
		public Inventory(O owner) => Owner = owner;

		protected virtual bool Move(I item, bool silent = false) {
			items.Add(item);
			return true;
		}

		public bool MoveItem(I item, bool silent = false) {
			item.ContainedInventory?.RemoveItem(item);
			bool res = Move(item, silent);
			item.ContainedInventory = this;
			return res;
		}

		void ICollection<I>.Add(I item) {
			if (!Move(item, false))
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

	public class WearablesInventory : Inventory<IWearable, ICanWear> {
		public WearablesInventory(ICanWear owner) : base(owner) { }

		protected override bool Move(IWearable wearable, bool silent = false) {
			bool failed = false;
			foreach (IWearable piece in items) {
				if (!wearable.IsCompatibleWith(piece)) {
					if (!silent) {
						if (!failed)
							Console.WriteLine($"Cannot wear {wearable.Name} together with:");
						string layers = (wearable.CoversLayers & piece.CoversLayers).FancyBitFlagEnum(out int count);
						string layerPlural = count != 1 ? "s" : "";
						string parts = (wearable.CoversParts & piece.CoversParts).FancyBitFlagEnum();
						Console.WriteLine($"  {piece.ListingName()}, they both cover the {layers} layer{layerPlural} on the {parts}");
					}
					failed = true;
				}
			}
			if (failed)
				return false;
			items.Add(wearable);
			return true;
		}
	}
}

