using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Inventory;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	[Serializable]
	public class ItemAction : IPerformableAction {
		[JsonIgnore]
		public IItem Item { get; set; }

		[JsonProperty("hasTarget")]
		public bool HasTarget { get; set; }

		[JsonProperty("callNames")]
		public string[] CallNames { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonIgnore]
		public string ListingName => $"{Item.Name}->{Name}";

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("executeDescription")]
		public string ExecuteDescription { get; set; }

		[JsonProperty("requirements")]
		public Requirements Requirements { get; set; }

		[JsonProperty("effects")]
		public Effects Effects { get; set; }

		[JsonIgnore]
		public string Listing {
			get {
				StringBuilder builder = new();
				builder.AppendLine(Description);
				builder.AppendLine();

				if (Effects.ConsumeSelf) {
					builder.AppendLine("{red}(Will Destroy Item!)");
				}
				if (Effects.TransferSelf.HasValue) {
					builder.AppendLine($"{{yellow}}(Transfers Item to {Effects.TransferSelf.Value})");
				}
				builder.AppendLine($"Requires: {(-Requirements.Stats).Listing}");
				if (Requirements.Items.Count > 0) {
					builder.AppendLine($"  Needed: {Requirements.Items}");
					foreach (KeyValuePair<string, int> pair in Requirements.Items) {
						builder.AppendLine($"    {Utils.AddSignAndColor(pair.Value, true, false)} {pair.Key}{(pair.Value > 1 ? "s" : "")}");
					}
				}
				if (Effects.ConsumeItems.Count > 0) {
					builder.AppendLine("  Items Consumed:");
					foreach (KeyValuePair<string, int> pair in Effects.ConsumeItems) {
						builder.AppendLine($"    {Utils.AddSignAndColor(-pair.Value)} {pair.Key}{(pair.Value > 1 ? "s" : "")}");
					}
				}

				builder.AppendLine();
				builder.AppendLine($"Effects: {Effects.Stats.Listing}");
				if (Effects.MeleeDamage != 0) {
					builder.AppendLine($"  Melee Damage: {Utils.AddSignAndColor(-Effects.MeleeDamage)}");
				}
				if (Effects.ProjectileDamage != 0) {
					builder.AppendLine($"  Projectile Damage: {Utils.AddSignAndColor(-Effects.ProjectileDamage)}");
				}
				if (Effects.ArmorPiercing != 0) {
					builder.AppendLine($"  Armor Piercing: {Utils.AddSignAndColor(Effects.ArmorPiercing * 100, true, false)}%");
				}
				if (Effects.HealSelf != 0) {
					builder.AppendLine($"  You Heal {Utils.AddSignAndColor(Effects.HealSelf)} health");
				}
				if (Effects.HealTarget != 0) {
					builder.AppendLine($"  Target Heals {Utils.AddSignAndColor(Effects.HealTarget)} health");
				}
				if (Effects.TransferItems.Count > 0) {
					builder.AppendLine("  Items Transferred:");
					foreach (KeyValuePair<string, Effects.ItemTransferEntry> pair in Effects.TransferItems) {
						builder.AppendLine($"    {pair.Value.Amount} {pair.Key}{(pair.Value.Amount > 1 ? "s" : "")} to {pair.Value.Location}");
					}
				}
				return builder.ToString();
			}
		}

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

		public bool CanAfford(TurnAction turnAction) {
			foreach ((string id, int amount) in Requirements.Items) {
				if (turnAction.performer.Inventory.GetItemCountById(id) < amount) {
					return false;
				}
			}

			return true;
		}

		public void Execute(TurnAction turnAction) {
			//consume items
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

			//transfer items
			ItemInventory GetTransferTarget(TransferLocation transferLocation) {
				return transferLocation switch {
					TransferLocation.Ground => turnAction.performer.location.items,
					TransferLocation.Target => turnAction.target.Inventory,
					_ => throw new InvalidOperationException()
				};
			}

			if (Effects.TransferSelf.HasValue) {
				GetTransferTarget(Effects.TransferSelf.Value).MoveItem(Item);
			}

			foreach ((string id, Effects.ItemTransferEntry itemTransferEntry) in Effects.TransferItems) {
				for (int i = 0; i < itemTransferEntry.Amount; i++) {
					if (turnAction.performer.Inventory.TryGetItemById(id, out IItem item)) {
						GetTransferTarget(itemTransferEntry.Location).MoveItem(item);
					}
				}
			}

			//heal
			turnAction.performer.health.Heal(Effects.HealSelf, turnAction.performer);
			turnAction.target?.health.Heal(Effects.HealTarget, turnAction.performer);

			//damage
			//TODO: handle damage types...
			turnAction.target?.health.TakeDamage(Effects.MeleeDamage, turnAction.performer);
			turnAction.target?.health.TakeDamage(Effects.ProjectileDamage, turnAction.performer);

			//tell the player what happend
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
