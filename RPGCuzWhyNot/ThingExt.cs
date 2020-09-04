

namespace RPGCuzWhyNot {
	public static class ThingExt {
		public static string DefaultListingName(IThing thing) {
			return $"{thing.Name} {{Magenta}}([{thing.CallName}])";
		}
	}
}

