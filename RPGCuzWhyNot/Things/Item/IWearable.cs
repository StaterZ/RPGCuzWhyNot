using RPGCuzWhyNot.Systems.HealthSystem;

namespace RPGCuzWhyNot.Things.Item {
	public interface IWearable : IItem, IHealthChangeModifier {
		float MultiplicativeProtection { get; set; }
		int AdditiveProtection { get; set; }
		float MultiplicativeBuff { get; set; }
		int AdditiveBuff { get; set; }

		WearableSlots CoveredParts { get; set; }
		WearableLayers CoveredLayers { get; set; }

		string ListingWithStats { get; }
	}
}

