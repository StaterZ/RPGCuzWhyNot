using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Things.Item {
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

		[JsonPropertyName("consumeSelf")]
		public bool ConsumeSelf { get; set; }

		[JsonPropertyName("consumeItems")]
		public Dictionary<string, int> ConsumeItems { get; set; }

		[JsonPropertyName("transferSelf")]
		public TransferLocation? TransferSelf { get; set; }

		[JsonPropertyName("transferItems")]
		public Dictionary<string, (TransferLocation location, int amount)> TransferItems { get; set; }

		[JsonPropertyName("healSelf")]
		public int HealSelf { get; set; }

		[JsonPropertyName("healTarget")]
		public int HealTarget { get; set; }

		public Effects() {
			Stats = new Stats();
			ConsumeItems = new Dictionary<string, int>();
			TransferItems = new Dictionary<string, (TransferLocation location, int amount)>();
		}

		public Effects(Stats stats, int meleeDamage, int projectileDamage, float armorPiercing, bool consumeSelf, Dictionary<string, int> consumeItems, TransferLocation? transferSelf, Dictionary<string, (TransferLocation location, int amount)> transferItems, int healSelf, int healTarget) {
			Stats = stats;
			MeleeDamage = meleeDamage;
			ProjectileDamage = projectileDamage;
			ArmorPiercing = armorPiercing;
			ConsumeSelf = consumeSelf;
			ConsumeItems = consumeItems;
			TransferSelf = transferSelf;
			TransferItems = transferItems;
			HealSelf = healSelf;
			HealTarget = healTarget;
		}
	}
}