namespace RPGCuzWhyNot.Inventory.Item {
	public static class WearableExt {
		public static bool IsCompatibleWith(this IWearable self, IWearable other) {
			return (self.CoveredLayers & other.CoveredLayers) == 0 || (self.CoveredParts & other.CoveredParts) == 0;
		}

		public static string DefaultListingWithStats(IWearable self) {
			string name = self.ListingName;
			return self.Defense != 0 ? $"{name}  {ConsoleUtils.FormatInt(self.Defense)} Defense" : name;
		}
	}
}

