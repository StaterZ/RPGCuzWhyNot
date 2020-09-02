

namespace RPGCuzWhyNot {
	public interface IWieldable : IItem {
		int HandsRequired { get; }
		int MeleeDamage { get; }
	}

	public static class WieldableExt {
		public static string ListingWithStats(this IWieldable w) {
			return $"{w.ListingName()}  {w.MeleeDamage} Melee Damage";
		}
	}
}

