using System;

namespace RPGCuzWhyNot.Inventory {
	public class WieldablesInventory : InventoryBase<IWieldable, ICanWield> {
		public int HandsAvailable { get; set; }

		public WieldablesInventory(ICanWield owner, int handsAvailable) : base(owner) => HandsAvailable = handsAvailable;

		protected override bool CheckMove(IWieldable item, bool silent) {
			bool res = GetNumberOfHandsInUse() + item.HandsRequired <= HandsAvailable;
			if (!res && !silent) {
				Console.WriteLine($"There are too few hands to wield any more things");
			}

			return res;
		}

		public int GetNumberOfHandsInUse() {
			int n = 0;
			foreach (IWieldable wieldable in items) {
				n += wieldable.HandsRequired;
			}

			return n;
		}
	}
}