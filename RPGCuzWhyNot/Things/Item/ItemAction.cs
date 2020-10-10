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
					turnAction.performer.Inventory.GetItemById(item.Key)?.Destroy();
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
					IItem item = turnAction.performer.Inventory.GetItemById(pair.Key);
					if (item != null) {
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
			if (turnAction.performer.race is Humanoid humanoidPerformer) {
				executeDescription = executeDescription.Replace("performer_referal_subjectPronoun", humanoidPerformer.gender.referral.subjectPronoun);
				executeDescription = executeDescription.Replace("performer_referal_objectPronoun", humanoidPerformer.gender.referral.objectPronoun);
				executeDescription = executeDescription.Replace("performer_referal_possessiveAdjective", humanoidPerformer.gender.referral.possessiveAdjective);
				executeDescription = executeDescription.Replace("performer_referal_possessivePronoun", humanoidPerformer.gender.referral.possessivePronoun);
				executeDescription = executeDescription.Replace("performer_referal_reflexivePronoun", humanoidPerformer.gender.referral.reflexivePronoun);
			}

			if (turnAction.target?.race is Humanoid humanoidTarget) {
				executeDescription = executeDescription.Replace("target_referal_subjectPronoun", humanoidTarget.gender.referral.subjectPronoun);
				executeDescription = executeDescription.Replace("target_referal_objectPronoun", humanoidTarget.gender.referral.objectPronoun);
				executeDescription = executeDescription.Replace("target_referal_possessiveAdjective", humanoidTarget.gender.referral.possessiveAdjective);
				executeDescription = executeDescription.Replace("target_referal_possessivePronoun", humanoidTarget.gender.referral.possessivePronoun);
				executeDescription = executeDescription.Replace("target_referal_reflexivePronoun", humanoidTarget.gender.referral.reflexivePronoun);
			}

			return executeDescription;
		}
	}
}