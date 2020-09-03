namespace RPGCuzWhyNot.Inventory.Item {
	public interface IWearable : IItem {
		int Defense { get; set; }
		// string WornDescription { get; }
		BodyParts CoveredParts { get; set; }
		WearableLayers CoveredLayers { get; set; }

		string ListingWithStats();
	}
}

