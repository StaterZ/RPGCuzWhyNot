using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.Inventory {
	public class WearablesInventory : InventoryBase<IWearable, ICanWear> {
		public WearablesInventory(ICanWear owner) : base(owner) { }

		protected override bool CheckMove(IWearable wearable, bool silent) {
			bool failed = false;
			foreach (IWearable piece in items) {
				if (wearable.IsCompatibleWith(piece)) continue;

				if (!silent) {
					if (!failed) {
						Terminal.WriteLine($"Cannot wear {wearable.Name} together with:");
						NumericCallNames.Clear();
					}
					string layers = (wearable.CoveredLayers & piece.CoveredLayers).FancyBitFlagEnum(out int count).ToLowerInvariant();
					string layerPlural = count != 1 ? "s" : "";
					string parts = (wearable.CoveredParts & piece.CoveredParts).FancyBitFlagEnum().ToLowerInvariant();
					Terminal.WriteLine($"  {NumericCallNames.HeadingOfAdd(piece)}{piece.ListingName}, they both cover the {layers} layer{layerPlural} on the {parts}");
				}
				failed = true;
			}
			return !failed;
		}
	}
}

