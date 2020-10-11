using System;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	public static class WearableExt {
		public static bool IsCompatibleWith(this IWearable self, IWearable other) {
			return (self.CoveredLayers & other.CoveredLayers) == 0 || (self.CoveredParts & other.CoveredParts) == 0;
		}

		public static string DefaultListingWithStats(this IWearable self) {
			return $"{self.ListingName} (Def: {Utils.FormatInt(self.ConstProtection)}C, {Utils.FormatFloat(self.FractionalProtection)}F) (Buf: {Utils.FormatInt(self.ConstBuff)}C, {Utils.FormatFloat(self.FractionalBuff)}F)";
		}

		public static int DefaultOnDamageModify(this IWearable self, int amount) {
			return (int)Math.Ceiling(Math.Max(amount - self.ConstProtection, 0) * (1 - self.FractionalProtection));
		}

		public static int DefaultOnHealModify(this IWearable self, int amount) {
			return (int)Math.Ceiling(Math.Max(amount + self.ConstBuff, 0) * (self.FractionalBuff - 1));
		}
	}
}

