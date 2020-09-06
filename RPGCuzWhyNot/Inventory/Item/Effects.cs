using System;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Inventory.Item {
	[Serializable]
	public class Effects {
		[JsonPropertyName("stats")]
		public Stats Stats { get; set; }

		[JsonPropertyName("meleeDamage")]
		public int MeleeDamage { get; set; }

		[JsonPropertyName("projectileDamage")]
		public int ProjectileDamage { get; set; }

		[JsonPropertyName("armorPiercing")]
		public float ArmorPiercing { get; set; }

		[JsonPropertyName("consumeItem")]
		public bool ConsumeItem { get; set; }

		public Effects() {
			Stats = new Stats();
		}

		public Effects(Stats stats, int meleeDamage, int projectileDamage, float armorPiercing, bool consumeItem) {
			Stats = stats;
			MeleeDamage = meleeDamage;
			ProjectileDamage = projectileDamage;
			ArmorPiercing = armorPiercing;
			ConsumeItem = consumeItem;
		}
	}
}