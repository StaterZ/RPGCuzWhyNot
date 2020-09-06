namespace RPGCuzWhyNot.Inventory.Item {
	public class WearableItemWithInventory : ItemWithInventory, IWearable {
		public int Defense { get; set; }
		public WearableSlots CoveredParts { get; set; }
		public WearableLayers CoveredLayers { get; set; }

		public WearableItemWithInventory(string name, string callName, string descriptionInInventory, string descriptionOnGround)
			: base(name, callName, descriptionInInventory, descriptionOnGround) { }
	}
}
