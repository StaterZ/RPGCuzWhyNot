using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public sealed class ItemPrototype : Prototype {
		[JsonProperty("inventoryDescription", Required = Required.Always)]
		public string DescriptionInInventory { get; set; }

		[JsonProperty("groundDescription", Required = Required.Always)]
		public string DescriptionOnGround { get; set; }

		[JsonProperty("wieldable")]
		public WieldableProps Wieldable { get; set; }

		[JsonProperty("wearable")]
		public WearableProps Wearable { get; set; }

		[JsonPropertyName("handsRequired")]
		public int? HandsRequired { get; set; }

		[JsonPropertyName("requirements")]
		public Requirements UsageRequirements { get; set; }

		[JsonPropertyName("actions")]
		public ItemAction[] ItemActions { get; set; }

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
		
		[JsonProperty("inventory")]
		public InventoryProps Inventory { get; set; }


		/// <summary>
		/// Create an instance of the prototype.
		/// </summary>
		public IItem Create() {
			IItem item;
			if (Inventory != null && Wearable != null)
				item = new WearableItemWithInventory(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (Inventory != null)
				item = new ItemWithInventory(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (Wearable != null && Wieldable != null)
				item = new WieldableWearableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (Wearable != null)
				item = new WearableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else if (Wieldable != null)
				item = new WieldableItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);
			else
				item = new SimpleItem(Name, CallName, DescriptionInInventory, DescriptionOnGround);

			if (Wearable != null) {
				IWearable wearable = (IWearable)item;
				wearable.Defense = Wearable.Defense;
				wearable.CoveredParts = Wearable.CoveredParts;
				wearable.CoveredLayers = Wearable.CoveredLayers;
			}

			if (Wieldable != null) {
				IWieldable wieldable = (IWieldable)item;
				wieldable.HandsRequired = HandsRequired ?? 0;
				wieldable.UsageRequirements = UsageRequirements ?? new Requirements();
			}

			if (Inventory != null) {
				IItemWithInventory inventory = (IItemWithInventory)item;
				inventory.WeightFraction = Inventory.WeightFraction;
			}

			item.ItemActions = ItemActions ?? Array.Empty<ItemAction>();
			foreach (ItemAction action in item.ItemActions) {
				action.Item = item;
			}

			item.Prototype = this;

			return item;
		}


		[JsonObject(ItemRequired = Required.Always)]
		public class WieldableProps {
			[JsonProperty("handsRequired")]
			public int HandsRequired { get; set; }

			[JsonProperty("meleeDamage")]
			public int MeleeDamage { get; set; }
		}

		[JsonObject(ItemRequired = Required.Always)]
		public class WearableProps {
			[JsonProperty("defense")]
			public int Defense { get; set; }

			[JsonProperty("coveredParts")]
			public WearableSlots CoveredParts { get; set; }

			[JsonProperty("coveredLayers")]
			public WearableLayers CoveredLayers { get; set; }
		}

		[JsonObject(ItemRequired = Required.Always)]
		public class InventoryProps {
			[JsonProperty("weightFraction")]
			public Fraction WeightFraction { get; set; }
		}
	}
}