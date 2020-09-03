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

		public static string FancyBitFlagEnum<E>(this E e) where E : Enum => FancyBitFlagEnum(e, out _);

		public static string FancyBitFlagEnum<E>(this E e, out int count) where E : Enum {
			List<string> res = new List<string>();
			foreach (Enum r in Enum.GetValues(typeof(E))) {
				if (e.HasFlag(r)) {
					res.Add(r.ToString());
				}
			}
			count = res.Count;
			switch (res.Count) {
				case 0: return "";
				case 1: return res[0];
				case 2: return $"{res[0]} and {res[1]}";
				default:
					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < res.Count - 1; ++i) {
						sb.Append(res[i]);
						sb.Append(", ");
					}
					sb.Append("and ");
					sb.Append(res[res.Count - 1]);
					return sb.ToString();
			}
		}
	}
}

