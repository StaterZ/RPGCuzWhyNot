using RPGCuzWhyNot.Systems.HealthSystem;

namespace RPGCuzWhyNot.Things.Item {
	public interface IWearable : IItem, IHealthChangeModifier {
		float MultiplicativeProtection { get; set; }
		int AdditiveProtection { get; set; }
		float MultiplicativeHealModifier { get; set; }
		int AdditiveHealModifier { get; set; }

		WearableSlots CoveredParts { get; set; }
		WearableLayers CoveredLayers { get; set; }

		string ListingWithStats { get; }
	}
}

