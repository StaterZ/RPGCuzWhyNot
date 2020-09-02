

namespace RPGCuzWhyNot {
	public class WearableItem : SimpleItem, IWearable {
		public int Defense { get; set; } = 1;
		public BodyParts CoverdParts { get; set; } = BodyParts.Head;
		public WearableLayers CoverdLayers { get; set; } = WearableLayers.Outer;

		public WearableItem(string name, string callName, string descInInv, string descOnGround = default)
			: base(name, callName, descInInv, descOnGround) { }
	}
}

