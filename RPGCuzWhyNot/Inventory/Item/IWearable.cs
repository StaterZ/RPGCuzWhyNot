namespace RPGCuzWhyNot.Inventory.Item {
	public interface IWearable : IItem {
		int Defense { get; }
		// string WornDescription { get; }
		BodyParts CoveredParts { get; }
		WearableLayers CoveredLayers { get; }
	}
}

