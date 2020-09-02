namespace RPGCuzWhyNot.Inventory.Item {
	public static class WieldableExt {
		public static string ListingWithStats(this IWieldable w) {
			return $"{w.ListingName()}  {w.MeleeDamage} Melee Damage";
		}
	}
}

