using RPGCuzWhyNot.Systems.HealthSystem;

namespace RPGCuzWhyNot.Things.Item {
	public interface IWearable : IItem, IHealthChangeModifier {
		float FractionalProtection { get; set; }
		int ConstProtection { get; set; }
		float FractionalBuff { get; set; }
		int ConstBuff { get; set; }

		WearableSlots CoveredParts { get; set; }
		WearableLayers CoveredLayers { get; set; }

		string ListingWithStats { get; }
	}
}

