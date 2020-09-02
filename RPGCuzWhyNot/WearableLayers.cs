using System;

namespace RPGCuzWhyNot {
	[Flags]
	public enum WearableLayers {
		Inner = 1 << 0, // regular clothing
		Middle = 1 << 1, // light armor, like chainmail or leather
		Outer = 1 << 2, // heavy armor, like full plate armor
	}
}