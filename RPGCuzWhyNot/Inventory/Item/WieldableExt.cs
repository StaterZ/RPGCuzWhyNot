namespace RPGCuzWhyNot.Inventory.Item {
	public static class WieldableExt {
		public static string DefaultListingWithStats(IWieldable w) {
			return $"{w.ListingName}  {ConsoleUtils.FormatInt(w.HandsRequired)} hands required.";
		}
	}
}

