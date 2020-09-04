using System;
using System.Collections.Generic;
using System.Text;

namespace RPGCuzWhyNot.Inventory.Item {
	public static class WearableExt {
		public static bool IsCompatibleWith(this IWearable self, IWearable other) {
			return (self.CoveredLayers & other.CoveredLayers) == 0 || (self.CoveredParts & other.CoveredParts) == 0;
		}

		public static string ListingWithStats(this IWearable self) {
			string name = self.ListingName();
			return self.Defense != 0 ? $"{name} {self.Defense} Defense" : name;
		}
	}
}

