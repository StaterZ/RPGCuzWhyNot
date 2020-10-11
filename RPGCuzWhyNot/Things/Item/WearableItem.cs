namespace RPGCuzWhyNot.Things.Item {
	public class WearableItem : SimpleItem, IWearable {
		public float FractionalProtection { get; set; }
		public int ConstProtection { get; set; }
		public float FractionalBuff { get; set; }
		public int ConstBuff { get; set; }
		public WearableSlots CoveredParts { get; set; }
		public WearableLayers CoveredLayers { get; set; }

		public WearableItem(string name, string callName, string descInInv, string descOnGround = default)
			: base(name, callName, descInInv, descOnGround) { }

		public virtual string ListingWithStats => this.DefaultListingWithStats();

		public int OnDamageModify(int amount) {
			return this.DefaultOnDamageModify(amount);
		}

		public int OnHealModify(int amount) {
			return this.DefaultOnHealModify(amount);
		}
	}
}

