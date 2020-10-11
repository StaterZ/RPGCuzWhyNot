namespace RPGCuzWhyNot.Things.Item {
	public class WearableItem : SimpleItem, IWearable {
		public float MultiplicativeProtection { get; set; }
		public int AdditiveProtection { get; set; }
		public float MultiplicativeBuff { get; set; }
		public int AdditiveBuff { get; set; }
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

