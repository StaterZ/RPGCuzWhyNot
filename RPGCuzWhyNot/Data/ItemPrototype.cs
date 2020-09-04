using System;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Data {
	[Serializable]
	public sealed class ItemPrototype : Prototype {
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("inventoryDescription")]
		public string DescriptionInInventory { get; set; }

		[JsonPropertyName("groundDescription")]
		public string DescriptionOnGround { get; set; }

		[JsonPropertyName("wieldable")]
		public bool Wieldable { get; set; }

		[JsonPropertyName("wearable")]
		public bool Wearable { get; set; }

		[JsonPropertyName("handsRequired")]
		public int HandsRequired { get; set; }

		[JsonPropertyName("meleeDamage")]
		public int MeleeDamage { get; set;  }

		[JsonPropertyName("defense")]
		public int Defense { get; set; }

		[JsonPropertyName("coveredParts"), JsonConverter(typeof(JsonEnumConverter))]
		public BodyParts CoveredParts { get; set; }

		[JsonPropertyName("coveredLayers"), JsonConverter(typeof(JsonEnumConverter))]
		public WearableLayers CoveredLayers { get; set; }

		public IItem Create() {
			IItem item;
			if (Wearable && Wieldable)
				item = new WieldableWearableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (Wearable)
				item = new WearableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (Wieldable)
				item = new WieldableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else
				item = new SimpleItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);

			if (Wearable) {
				IWearable wearable = (IWearable)item;
				wearable.Defense = Defense;
				wearable.CoveredParts = CoveredParts;
				wearable.CoveredLayers = CoveredLayers;
			}

			if (Wieldable) {
				IWieldable wieldable = (IWieldable)item;
				wieldable.HandsRequired = HandsRequired;
				wieldable.MeleeDamage = MeleeDamage;
			}

			return item;
		}
	}
}