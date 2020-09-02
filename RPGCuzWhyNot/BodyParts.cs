using System;

namespace RPGCuzWhyNot {
	// the order is deliberate, lower values have higher priorities, used when bit-twiddling
	[Flags]
	public enum BodyParts {
		Chest = 1 << 0, // shirts
		Head = 1 << 1, // hats
		Arms = 1 << 2, // long-sleeved clothing
		Legs = 1 << 3, // pants
		Hands = 1 << 4, // gloves
		Feet = 1 << 5, // shoes
		Face = 1 << 6, // masks
		Neck = 1 << 7, // necklaces
		Wrists = 1 << 8, // bracelets
	}
}