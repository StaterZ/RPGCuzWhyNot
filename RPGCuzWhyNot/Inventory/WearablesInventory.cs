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
						ConsoleUtils.SlowWriteLine($"Cannot wear {wearable.Name} together with:");
					string layers = (wearable.CoveredLayers & piece.CoveredLayers).FancyBitFlagEnum(out int count).ToLowerInvariant();
					string layerPlural = count != 1 ? "s" : "";
					string parts = (wearable.CoveredParts & piece.CoveredParts).FancyBitFlagEnum().ToLowerInvariant();
					ConsoleUtils.SlowWriteLine($"  {piece.ListingName()}, they both cover the {layers} layer{layerPlural} on the {parts}");
				}
				failed = true;
			}
			return !failed;
		}
	}
}

