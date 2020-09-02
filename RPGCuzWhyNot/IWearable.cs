using System;
using System.Text;
using System.Collections.Generic;

namespace RPGCuzWhyNot {
	public interface IWearable : IItem {
		int Defense { get; }
		// string WornDescription { get; }
		BodyParts CoversParts { get; }
		WearableLayers CoversLayers { get; }
	}

	public static class WearableExt {
		public static bool IsCompatibleWith(this IWearable self, IWearable other) {
			return ((self.CoversLayers & other.CoversLayers) == 0) || ((self.CoversParts & other.CoversParts) == 0);
		}

		public static string ListingName(this IWearable self) {
			return $"{self.Name} [{self.Callname}]";
		}

		public static string ListingWithStats(this IWearable self) {
			string name = self.ListingName();
			if (self.Defense != 0)
				return $"{name}  {self.Defense} Defense";
			return name;
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

	// the order is deliberate, lower values have higher prorities, used when bit-twiddling
	[Flags]
	public enum BodyParts {
		Chest = 1 << 0, // shirts
		Head = 1 << 1, // hats
		Arms = 1 << 2, // long-sleeved clothing
		Legs = 1 << 3, // pants
		Hands = 1 << 4, // gloves
		Feet = 1 << 5, // shoes
		Face = 1 << 6, // masks
		Neck = 1 << 7, // necklaces
		Wrists = 1 << 8, // bracelets
	}

	[Flags]
	public enum WearableLayers {
		Inner = 1 << 0, // regular clothing
		Middle = 1 << 1, // light armor, like chainmail or leather
		Outer = 1 << 2, // heavy armor, like full plate armor
	}
}

