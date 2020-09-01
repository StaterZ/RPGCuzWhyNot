

namespace RPGCuzWhyNot {
	public class SimpleWearableItem : SimpleItem, IWearable {
		public int Defense { get; set; } = 1;
		public BodyParts CoversParts { get; set; } = BodyParts.Head;
		public WearableLayers CoversLayers { get; set; } = WearableLayers.Outer;

		public SimpleWearableItem(string name, string callname, string descInInv, string descOnGround = default) : base(name, callname, descInInv, descOnGround) { }
	}
}

