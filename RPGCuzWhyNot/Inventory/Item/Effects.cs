namespace RPGCuzWhyNot.Inventory.Item {
	public class Effects {
		public Stats Stats { get; }
		public int MeleeDamage { get; }
		public int ProjectileDamage { get; }
		public float ArmorPierceing { get; }
		public bool ConsumeItem { get; }

		public Effects() { }

		public Effects(Stats stats, int meleeDamage, int projectileDamage, float armorPierceing, bool consumeItem) {
			Stats = stats;
			MeleeDamage = meleeDamage;
			ProjectileDamage = projectileDamage;
			ArmorPierceing = armorPierceing;
			ConsumeItem = consumeItem;
		}
	}
}