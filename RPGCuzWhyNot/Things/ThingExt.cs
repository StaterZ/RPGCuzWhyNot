namespace RPGCuzWhyNot.Things {
	public static class ThingExt {
		public static string DefaultListingName(IThing thing) {
			return $"{thing.Name} {thing.FormattedCallName}";
		}
	}
}

