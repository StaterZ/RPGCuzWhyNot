using System;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Data {
	[Serializable]
	public sealed class ItemPrototype : Prototype {
		[JsonPropertyName("inventoryDescription")]
		public string DescriptionInInventory { get; set; }

		[JsonPropertyName("groundDescription")]
		public string DescriptionOnGround { get; set; }

		[JsonPropertyName("wieldable")]
		public bool IsWieldable { get; set; }

		[JsonPropertyName("wearable")]
		public bool IsWearable { get; set; }

		[JsonPropertyName("handsRequired")]
		public int HandsRequired { get; set; }

		[JsonPropertyName("meleeDamage")]
		public int MeleeDamage { get; set;  }

		[JsonPropertyName("defense")]
		public int Defense { get; set; }

		[JsonPropertyName("hasInventory")]
		public bool HasInventory { get; set; }

		[JsonPropertyName("weightFractionNumerator")]
		public int WeightFractionNumerator { get; set; }

		[JsonPropertyName("weightFractionDenominator")]
		public int WeightFractionDenominator { get; set; }

		[JsonPropertyName("coveredParts"), JsonConverter(typeof(JsonEnumConverter))]
		public WearableSlots CoveredParts { get; set; }

		[JsonPropertyName("coveredLayers"), JsonConverter(typeof(JsonEnumConverter))]
		public WearableLayers CoveredLayers { get; set; }

		/// <summary>
		/// Create an instance of the prototype.
		/// </summary>
		public IItem Create() {
			IItem item;
			if (HasInventory && IsWearable)
				item = new WearableItemWithInventory(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (HasInventory)
				item = new ItemWithInventory(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (IsWearable && IsWieldable)
				item = new WieldableWearableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (IsWearable)
				item = new WearableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (IsWieldable)
				item = new WieldableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else
				item = new SimpleItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);

			if (IsWearable) {
				IWearable wearable = (IWearable)item;
				wearable.Defense = Defense;
				wearable.CoveredParts = CoveredParts;
				wearable.CoveredLayers = CoveredLayers;
			}

			if (IsWieldable || HasInventory) {
				IWieldable wieldable = (IWieldable)item;
				wieldable.HandsRequired = HandsRequired;
				wieldable.MeleeDamage = MeleeDamage;
			}

			if (HasInventory) {
				IItemWithInventory inventory = (IItemWithInventory)item;
				inventory.WeightFraction = new Fraction(WeightFractionNumerator, WeightFractionDenominator);
			}

			item.Prototype = this;
			return item;
		}
	}
}