using System;

namespace RPGCuzWhyNot.Things.Item {
	// the order is deliberate, lower values have higher priorities, used when bit-twiddling
	[Flags]
	public enum WearableSlots {
		Chest = 1 << 0, // shirts
		Back = 1 << 1, // backpacks
		Head = 1 << 2, // hats
		Arms = 1 << 3, // long-sleeved clothing
		Legs = 1 << 4, // pants
		Hands = 1 << 5, // gloves
		Feet = 1 << 6, // shoes
		Face = 1 << 7, // masks
		Neck = 1 << 8, // necklaces
		Wrists = 1 << 9, // bracelets
	}
}