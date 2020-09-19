using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	public static class WieldableExt {
		public static string DefaultListingWithStats(IWieldable w) {
			return $"{w.ListingName}  {ConsoleUtils.FormatInt(w.HandsRequired)} hands required.";
		}
	}
}

