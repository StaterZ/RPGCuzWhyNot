using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Inventory;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;

namespace RPGCuzWhyNot.Things.Item {
	[Serializable]
	public class ItemAction : IPerformableAction {
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

		public void Execute(TurnAction turnAction) {
			//consume
			if (Effects.ConsumeSelf) {
				Item.Destroy();
			}
			foreach (KeyValuePair<string, int> item in Effects.ConsumeItems) {
				for (int i = 0; i < item.Value; i++) {
					if (turnAction.performer.Inventory.TryGetItemById(item.Key, out IItem result)) {
						result.Destroy();
					}
				}
			}

			//transfer
			ItemInventory GetTransferTarget(TransferLocation transferLocation) {
				switch (transferLocation) {
					case TransferLocation.Ground:
						return turnAction.performer.location.items;
					case TransferLocation.Target:
						return turnAction.target.Inventory;
					default:
						throw new InvalidOperationException();
				}
			}

			if (Effects.TransferSelf.HasValue) {
				GetTransferTarget(Effects.TransferSelf.Value).MoveItem(Item);
			}
			foreach (KeyValuePair<string, (TransferLocation location, int amount)> pair in Effects.TransferItems) { 
				for (int i = 0; i < pair.Value.amount; i++) {
					if (turnAction.performer.Inventory.TryGetItemById(pair.Key, out IItem item)) {
						GetTransferTarget(pair.Value.location).MoveItem(item);
					}
				}
			}

			//heal
			turnAction.performer.health.Heal(Effects.HealSelf, turnAction.performer);
			turnAction.target?.health.Heal(Effects.HealTarget, turnAction.performer);

			string formattedExecuteDescription = FormatExecuteDescription(turnAction, ExecuteDescription);
			

			Terminal.WriteLine($"[{ListingName}] {formattedExecuteDescription}");
		}

		private static string FormatExecuteDescription(TurnAction turnAction, string executeDescription) {
			if (turnAction.performer != null) {
				executeDescription = executeDescription.Replace("<performer_name>", turnAction.performer.Name);
				executeDescription = executeDescription.Replace("<performer_referal_subjectPronoun>", turnAction.performer.Name);
				executeDescription = executeDescription.Replace("<performer_referal_objectPronoun>", turnAction.performer.Referral.objectPronoun);
				executeDescription = executeDescription.Replace("<performer_referal_possessiveAdjective>", turnAction.performer.Referral.possessiveAdjective);
				executeDescription = executeDescription.Replace("<performer_referal_possessivePronoun>", turnAction.performer.Referral.possessivePronoun);
				executeDescription = executeDescription.Replace("<performer_referal_reflexivePronoun>", turnAction.performer.Referral.reflexivePronoun);
			}

			if (turnAction.target != null) {
				executeDescription = executeDescription.Replace("<target_name>", turnAction.target.Name);
				executeDescription = executeDescription.Replace("<target_referal_subjectPronoun>", turnAction.target.Referral.subjectPronoun);
				executeDescription = executeDescription.Replace("<target_referal_objectPronoun>", turnAction.target.Referral.objectPronoun);
				executeDescription = executeDescription.Replace("<target_referal_possessiveAdjective>", turnAction.target.Referral.possessiveAdjective);
				executeDescription = executeDescription.Replace("<target_referal_possessivePronoun>", turnAction.target.Referral.possessivePronoun);
				executeDescription = executeDescription.Replace("<target_referal_reflexivePronoun>", turnAction.target.Referral.reflexivePronoun);
			}

			return executeDescription;
		}
	}
}