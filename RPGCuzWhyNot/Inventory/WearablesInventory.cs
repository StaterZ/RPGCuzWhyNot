using System;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public class WearablesInventory : InventoryBase<IWearable, ICanWear> {
		public WearablesInventory(ICanWear owner) : base(owner) { }

		protected override bool CheckMove(IWearable wearable, bool silent) {
			bool failed = false;
			foreach (IWearable piece in items) {
				if (wearable.IsCompatibleWith(piece)) continue;

				if (!silent) {
					if (!failed)
						Console.WriteLine($"Cannot wear {wearable.Name} together with:");
					string layers = (wearable.CoverdLayers & piece.CoverdLayers).FancyBitFlagEnum(out int count);
					string layerPlural = count != 1 ? "s" : "";
					string parts = (wearable.CoverdParts & piece.CoverdParts).FancyBitFlagEnum();
					Console.WriteLine($"  {WearableExt.ListingName(piece)}, they both cover the {layers} layer{layerPlural} on the {parts}");
				}
				failed = true;
			}
			return !failed;
		}
	}
}