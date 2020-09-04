namespace RPGCuzWhyNot.Inventory.Item {
	public static class WieldableExt {
		public static string ListingWithStats(this IWieldable w) {
			string plus = w.MeleeDamage > 0 ? "+" : "";
			return $"{w.ListingName()}  {plus}{w.MeleeDamage} Melee Damage";
		}
	}
}

