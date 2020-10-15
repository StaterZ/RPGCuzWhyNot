using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RPGCuzWhyNot.Things.Item {
	[Serializable]
	public class Effects {
		[JsonProperty("stats")]
		public Stats Stats { get; set; }

		[JsonProperty("meleeDamage")]
		public int MeleeDamage { get; set; }

		[JsonProperty("projectileDamage")]
		public int ProjectileDamage { get; set; }

		[JsonProperty("armorPiercing")]
		public float ArmorPiercing { get; set; }

		[JsonProperty("consumeSelf")]
		public bool ConsumeSelf { get; set; }

		[JsonProperty("consumeItems")]
		public Dictionary<string, int> ConsumeItems { get; set; }

		[JsonProperty("transferSelf")]
		public TransferLocation? TransferSelf { get; set; }

		[JsonProperty("transferItems")]
		public Dictionary<string, ItemTransferEntry> TransferItems { get; set; }

		[JsonProperty("healSelf")]
		public int HealSelf { get; set; }

		[JsonProperty("healTarget")]
		public int HealTarget { get; set; }

		public Effects() {
			Stats = new Stats();
			ConsumeItems = new Dictionary<string, int>();
			TransferItems = new Dictionary<string, ItemTransferEntry>();
		}

		public Effects(Stats stats, int meleeDamage, int projectileDamage, float armorPiercing, bool consumeSelf, Dictionary<string, int> consumeItems, TransferLocation? transferSelf, Dictionary<string, ItemTransferEntry> transferItems, int healSelf, int healTarget) {
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

		[JsonObject(ItemRequired = Required.Always)]
		public struct ItemTransferEntry {
			[JsonProperty("location")]
			public TransferLocation Location { get; set; }

			[JsonProperty("amount")]
			public int Amount { get; set; }
		}
	}
}