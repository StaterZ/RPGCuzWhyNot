using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	public static class WieldableExt {
		public static string DefaultListingWithStats(IWieldable w) {
			return $"{w.ListingName}  {Utils.AddSignAndColor(-w.HandsRequired)} hands required.";
		}
	}
}

