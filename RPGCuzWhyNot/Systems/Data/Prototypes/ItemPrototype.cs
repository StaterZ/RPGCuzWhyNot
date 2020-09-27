using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public sealed class ItemPrototype : Prototype {
		[JsonProperty("inventoryDescription")]
		public string DescriptionInInventory { get; set; }

		[JsonProperty("groundDescription")]
		public string DescriptionOnGround { get; set; }

		// Wieldable

		[JsonProperty("wieldable")]
		public bool IsWieldable { get; set; }

		[JsonProperty("handsRequired")]
		public int? HandsRequired { get; set; }

		[JsonProperty("meleeDamage")]
		public int? MeleeDamage { get; set; }

		// Wearable

		[JsonProperty("wearable")]
		public bool IsWearable { get; set; }

		[JsonProperty("defense")]
		public int? Defense { get; set; }

		[JsonProperty("coveredParts")]
		public WearableSlots CoveredParts { get; set; }

		[JsonProperty("coveredLayers")]
		public WearableLayers CoveredLayers { get; set; }

		// Inventory

		[JsonProperty("hasInventory")]
		public bool HasInventory { get; set; }

		[JsonProperty("weightFraction")]
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