using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	public static class WieldableExt {
		public static string DefaultListingWithStats(IWieldable w) {
			if (w.HandsRequired == 0) {
				return $"{w.ListingName}";
			}

			return $"{w.ListingName}  {Utils.AddSignAndColor(-w.HandsRequired)} hands required.";
		}
	}
}

