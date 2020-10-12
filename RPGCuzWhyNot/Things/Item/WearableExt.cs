using System;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	public static class WearableExt {
		public static bool IsCompatibleWith(this IWearable self, IWearable other) {
			return (self.CoveredLayers & other.CoveredLayers) == 0 || (self.CoveredParts & other.CoveredParts) == 0;
		}

		public static string DefaultListingWithStats(this IWearable self) {
			return $"{self.ListingName} (Def: {Utils.AddSignAndColor(self.AdditiveProtection)}A, {Utils.AddSignAndColor(self.MultiplicativeProtection)}M) (Buf: {Utils.AddSignAndColor(self.AdditiveHealModifier)}A, {Utils.AddSignAndColor(self.MultiplicativeHealModifier)}M)";
		}

		public static int DefaultOnDamageModify(this IWearable self, int amount) {
			return (int)Math.Ceiling(Math.Max(amount - self.AdditiveProtection, 0) * (1 - self.MultiplicativeProtection));
		}

		public static int DefaultOnHealModify(this IWearable self, int amount) {
			return (int)Math.Ceiling(Math.Max(amount + self.AdditiveHealModifier, 0) * (self.MultiplicativeHealModifier + 1));
		}
	}
}

