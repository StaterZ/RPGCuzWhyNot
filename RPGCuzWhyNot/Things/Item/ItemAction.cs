using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Inventory;

namespace RPGCuzWhyNot.Things.Item {
	[Serializable]
	public class ItemAction : IPlannableAction {
		[JsonIgnore]
		public IItem Item { get; set; }

		[JsonPropertyName("hasTarget")]
		public bool HasTarget { get; set; }

		[JsonPropertyName("callNames")]
		public string[] CallNames { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonIgnore]
		public string ListingName => $"{Item.Name}->{Name}";

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("executeDescription")]
		public string ExecuteDescription { get; set; }

		[JsonPropertyName("requirements")]
		public Requirements Requirements { get; set; }

		[JsonPropertyName("effects")]
		public Effects Effects { get; set; }

		public ItemAction() {
			Requirements = new Requirements();
			Effects = new Effects();
		}

		public ItemAction(IItem item, bool hasTarget, string[] callNames, string name, string description, string executeDescription, Requirements requirements, Effects effects) {
			Item = item;

			HasTarget = hasTarget;
			CallNames = callNames;
			Name = name;
			Description = description;
			ExecuteDescription = executeDescription;
			Requirements = requirements;
			Effects = effects;
		}

		public void Execute(PlannedAction plannedAction) {
			//consume
			if (Effects.ConsumeSelf) {
				Item.Destroy();
			}
			foreach (KeyValuePair<string, int> item in Effects.ConsumeItems) {
				for (int i = 0; i < item.Value; i++) {
					plannedAction.performer.Inventory.GetItemById(item.Key)?.Destroy();
				}
			}

			//transfer
			ItemInventory GetTransferTarget(TransferLocation transferLocation) {
				switch (transferLocation) {
					case TransferLocation.Ground:
						return plannedAction.performer.location.items;
					case TransferLocation.Target:
						return plannedAction.target.Inventory;
					default:
						throw new InvalidOperationException();
				}
			}

			if (Effects.TransferSelf.HasValue) {
				GetTransferTarget(Effects.TransferSelf.Value).MoveItem(Item);
			}
			foreach (KeyValuePair<string, (TransferLocation location, int amount)> pair in Effects.TransferItems) { 
				for (int i = 0; i < pair.Value.amount; i++) {
					IItem item = plannedAction.performer.Inventory.GetItemById(pair.Key);
					if (item != null) {
						GetTransferTarget(pair.Value.location).MoveItem(item);
					}
				}
			}

			//heal
			plannedAction.performer.health.Heal(Effects.HealSelf, plannedAction.performer);
			plannedAction.target?.health.Heal(Effects.HealTarget, plannedAction.performer);

			Terminal.WriteLine($"[{ListingName}] {ExecuteDescription}");
		}
	}
}