namespace RPGCuzWhyNot.Things.Item {
	public static class WearableExt {
		public static bool IsCompatibleWith(this IWearable self, IWearable other) {
			return (self.CoveredLayers & other.CoveredLayers) == 0 || (self.CoveredParts & other.CoveredParts) == 0;
		}

		public static string DefaultListingWithStats(IWearable self) {
			string name = self.ListingName;
			string plus = self.Defense > 0 ? "+" : "";
			return self.Defense != 0 ? $"{name}  {plus}{self.Defense} Defense" : name;
		}
	}
}

