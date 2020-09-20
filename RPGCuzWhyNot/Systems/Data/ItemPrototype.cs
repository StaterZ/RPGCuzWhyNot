using System;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Data {
	[Serializable]
	public sealed class ItemPrototype : Prototype {
		[JsonPropertyName("inventoryDescription")]
		public string DescriptionInInventory { get; set; }

		[JsonPropertyName("groundDescription")]
		public string DescriptionOnGround { get; set; }

		// Wieldable

		[JsonPropertyName("wieldable")]
		public bool IsWieldable { get; set; }

		[JsonPropertyName("handsRequired")]
		public int? HandsRequired { get; set; }

		[JsonPropertyName("meleeDamage")]
		public int? MeleeDamage { get; set; }

		// Wearable

		[JsonPropertyName("wearable")]
		public bool IsWearable { get; set; }

		[JsonPropertyName("defense")]
		public int? Defense { get; set; }

		[JsonPropertyName("coveredParts"), JsonConverter(typeof(JsonEnumConverter))]
		public WearableSlots CoveredParts { get; set; }

		[JsonPropertyName("coveredLayers"), JsonConverter(typeof(JsonEnumConverter))]
		public WearableLayers CoveredLayers { get; set; }

		// Inventory

		[JsonPropertyName("hasInventory")]
		public bool HasInventory { get; set; }

		[JsonPropertyName("weightFraction"), JsonConverter(typeof(JsonFractionConverter))]
		public Fraction WeightFraction { get; set; }


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
				wearable.Defense = Defense ?? 0;
				wearable.CoveredParts = CoveredParts;
				wearable.CoveredLayers = CoveredLayers;
			}

			if (IsWieldable || HasInventory) {
				IWieldable wieldable = (IWieldable)item;
				wieldable.HandsRequired = HandsRequired ?? 0;
				wieldable.MeleeDamage = MeleeDamage ?? 0;
			}

			if (HasInventory) {
				IItemWithInventory inventory = (IItemWithInventory)item;
				inventory.WeightFraction = WeightFraction;
			}

			item.Prototype = this;
			return item;
		}
	}
}