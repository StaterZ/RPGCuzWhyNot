

namespace RPGCuzWhyNot {
	public class WearableItem : SimpleItem, IWearable {
		public int Defense { get; set; } = 1;
		public BodyParts CoversParts { get; set; } = BodyParts.Head;
		public WearableLayers CoversLayers { get; set; } = WearableLayers.Outer;

		public WearableItem(string name, string callname, string descInInv, string descOnGround = default)
			: base(name, callname, descInInv, descOnGround) { }
	}
}

