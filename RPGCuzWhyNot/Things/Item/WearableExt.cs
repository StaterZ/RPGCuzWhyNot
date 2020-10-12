using System;
using System.Text;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	public static class WearableExt {
		public static bool IsCompatibleWith(this IWearable self, IWearable other) {
			return (self.CoveredLayers & other.CoveredLayers) == 0 || (self.CoveredParts & other.CoveredParts) == 0;
		}

		public static string DefaultListingWithStats(this IWearable self) {
			StringBuilder builder = new StringBuilder();
			builder.Append(self.ListingName);

			bool ap = self.AdditiveProtection != 0;
			bool mp = self.MultiplicativeProtection != 0;
			bool ah = self.AdditiveHealModifier != 0;
			bool mh = self.MultiplicativeHealModifier != 0;
			bool p = ap || mp;
			bool h = ah || mh;

			if (p || h) {
				if (p) {
					builder.Append(" ");
					builder.Append("(Def: ");
					if (ap) {
						builder.Append(Utils.AddSignAndColor(self.AdditiveProtection));
						if (mp) {
							builder.Append(", ");
						}
					}
					if (mp) {
						builder.Append(Utils.AddSignAndColor(self.MultiplicativeProtection * 100, false));
						builder.Append("%");
					}
					builder.Append(")");
				}
				if (h) {
					builder.Append(" ");
					builder.Append("(Heal: ");
					if (ah) {
						builder.Append(Utils.AddSignAndColor(self.AdditiveHealModifier));
						if (mh) {
							builder.Append(", ");
						}
					}
					if (mh) {
						builder.Append(Utils.AddSignAndColor(self.MultiplicativeHealModifier * 100, false));
						builder.Append("%");
					}
					builder.Append(")");
				}
			}

			return builder.ToString();
		}

		public static int DefaultOnDamageModify(this IWearable self, int amount) {
			return (int)Math.Ceiling(Math.Max(amount - self.AdditiveProtection, 0) * (1 - self.MultiplicativeProtection));
		}

		public static int DefaultOnHealModify(this IWearable self, int amount) {
			return (int)Math.Ceiling(Math.Max(amount + self.AdditiveHealModifier, 0) * (self.MultiplicativeHealModifier + 1));
		}
	}
}

