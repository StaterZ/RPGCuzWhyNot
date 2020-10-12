namespace RPGCuzWhyNot.Things.Item {
	public class WearableItemWithInventory : ItemWithInventory, IWearable {
		public float MultiplicativeProtection { get; set; }
		public int AdditiveProtection { get; set; }
		public float MultiplicativeHealModifier { get; set; }
		public int AdditiveHealModifier { get; set; }
		public WearableSlots CoveredParts { get; set; }
		public WearableLayers CoveredLayers { get; set; }

		public WearableItemWithInventory(string name, string callName, string descriptionInInventory, string descriptionOnGround)
			: base(name, callName, descriptionInInventory, descriptionOnGround) { }

		public int OnDamageModify(int amount) {
			return this.DefaultOnDamageModify(amount);
		}

		public int OnHealModify(int amount) {
			return this.DefaultOnHealModify(amount);
		}
	}
}
