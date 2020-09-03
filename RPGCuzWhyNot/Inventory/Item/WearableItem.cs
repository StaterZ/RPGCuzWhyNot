namespace RPGCuzWhyNot.Inventory.Item {
	public class WearableItem : SimpleItem, IWearable {
		public int Defense { get; set; }
		public BodyParts CoveredParts { get; set; }
		public WearableLayers CoveredLayers { get; set; }

		public WearableItem(string name, string callName, string descInInv, string descOnGround = default)
			: base(name, callName, descInInv, descOnGround) { }

		public virtual string ListingWithStats() => WearableExt.ListingWithStats(this);
	}
}

