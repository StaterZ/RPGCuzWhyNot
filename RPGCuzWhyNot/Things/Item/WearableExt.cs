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

			bool hasAdditiveProtection = self.AdditiveProtection != 0;
			bool hasMultiplicativeProtection = self.MultiplicativeProtection != 0;
			bool hasAdditiveHealModifier = self.AdditiveHealModifier != 0;
			bool hasMultiplicativeHealModifier = self.MultiplicativeHealModifier != 0;
			bool hasAnyProtection = hasAdditiveProtection || hasMultiplicativeProtection;
			bool hasAnyHealModifier = hasAdditiveHealModifier || hasMultiplicativeHealModifier;

			if (hasAnyProtection || hasAnyHealModifier) {
				if (hasAnyProtection) {
					builder.Append(" ");
					builder.Append("(Def: ");
					if (hasAdditiveProtection) {
						builder.Append(Utils.AddSignAndColor(self.AdditiveProtection));
						if (hasMultiplicativeProtection) {
							builder.Append(", ");
						}
					}
					if (hasMultiplicativeProtection) {
						builder.Append(Utils.AddSignAndColor(self.MultiplicativeProtection * 100, false));
						builder.Append("%");
					}
					builder.Append(")");
				}
				if (hasAnyHealModifier) {
					builder.Append(" ");
					builder.Append("(Heal: ");
					if (hasAdditiveHealModifier) {
						builder.Append(Utils.AddSignAndColor(self.AdditiveHealModifier));
						if (hasMultiplicativeHealModifier) {
							builder.Append(", ");
						}
					}
					if (hasMultiplicativeHealModifier) {
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

