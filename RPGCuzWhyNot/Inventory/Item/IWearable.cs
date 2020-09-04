namespace RPGCuzWhyNot.Inventory.Item {
	public interface IWearable : IItem {
		int Defense { get; set; }
		// string WornDescription { get; }
		WearableSlots CoveredParts { get; set; }
		WearableLayers CoveredLayers { get; set; }

		string ListingWithStats { get; }
	}
}

