using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public sealed class ItemPrototype : ThingPrototype {
		[JsonProperty("inventoryDescription", Required = Required.Always)]
		public string DescriptionInInventory { get; set; }

		[JsonProperty("groundDescription", Required = Required.Always)]
		public string DescriptionOnGround { get; set; }

		[JsonProperty("wieldable")]
		public WieldableProps Wieldable { get; set; }

		[JsonProperty("wearable")]
		public WearableProps Wearable { get; set; }

		[JsonProperty("inventory")]
		public InventoryProps Inventory { get; set; }

		[JsonProperty("actions")]
		public ItemAction[] ItemActions { get; set; }


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
				wearable.AdditiveProtection = Wearable.AdditiveProtection ?? 0;
				wearable.MultiplicativeProtection = Wearable.MultiplicativeProtection ?? 0;
				wearable.AdditiveBuff = Wearable.AdditiveBuff ?? 0;
				wearable.MultiplicativeBuff = Wearable.MultiplicativeBuff ?? 0;
				wearable.CoveredParts = Wearable.CoveredParts;
				wearable.CoveredLayers = Wearable.CoveredLayers;
			}

			if (Wieldable != null) {
				IWieldable wieldable = (IWieldable)item;
				wieldable.HandsRequired = Wieldable.HandsRequired;
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
		}

		[JsonObject]
		public class WearableProps {
			[JsonProperty("multiplicativeProtection")]
			public float? MultiplicativeProtection { get; set; }

			[JsonProperty("additiveProtection")]
			public int? AdditiveProtection { get; set; }

			[JsonProperty("multiplicativeBuff")]
			public float? MultiplicativeBuff { get; set; }

			[JsonProperty("additiveBuff")]
			public int? AdditiveBuff { get; set; }

			[JsonProperty("coveredParts"), JsonRequired]
			public WearableSlots CoveredParts { get; set; }

			[JsonProperty("coveredLayers"), JsonRequired]
			public WearableLayers CoveredLayers { get; set; }
		}

		[JsonObject(ItemRequired = Required.Always)]
		public class InventoryProps {
			[JsonProperty("weightFraction")]
			public Fraction WeightFraction { get; set; }
		}
	}
}