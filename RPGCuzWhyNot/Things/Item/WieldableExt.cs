using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	public static class WieldableExt {
		public static string DefaultListingWithStats(IWieldable w) {
			return $"{w.ListingName}  {Utils.FormatInt(w.HandsRequired, false)} hands required.";
		}
	}
}

