namespace RPGCuzWhyNot.Inventory.Item {
	public class Effects {
		public readonly Stats Stats;
		public readonly int meleeDamage;
		public readonly int projectileDamage;
		public readonly float armorPierceing;
		public readonly bool consumeItem;

		public Effects(Stats stats, int meleeDamage, int projectileDamage, float armorPierceing, bool consumeItem) {
			this.Stats = stats;
			this.meleeDamage = meleeDamage;
			this.projectileDamage = projectileDamage;
			this.armorPierceing = armorPierceing;
			this.consumeItem = consumeItem;
		}
	}
}