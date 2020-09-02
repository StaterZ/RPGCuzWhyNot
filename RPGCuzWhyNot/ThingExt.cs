namespace RPGCuzWhyNot {
	public static class ThingExt {
		public static string ListingName(this IThing thing) {
			return $"{thing.Name} [{thing.CallName}]";
		}
	}
}