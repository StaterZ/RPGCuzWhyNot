using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public class WearableItem : SimpleItem, IWearable {
		public int Defense { get; set; }
		public BodyParts CoverdParts { get; set; }
		public WearableLayers CoverdLayers { get; set; }

		public WearableItem(string name, string callName, string descInInv, string descOnGround = default)
			: base(name, callName, descInInv, descOnGround) { }
	}
}

