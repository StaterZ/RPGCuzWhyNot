using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Things.Item;
using System;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Inventory.Item {
	[Serializable]
	public class ItemAction : IPlannableAction {
		public IItem item;

		[JsonPropertyName("callNames")]
		public string[] CallNames { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		//todo: make json loader thing set item when creating object
		//public string ListingName => $"{item.ListingName}->{Name}";
		public string ListingName => throw new NotImplementedException();

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

		public ItemAction(IItem item, string[] callNames, string name, string description, string executeDescription, Requirements requirements, Effects effects) {
			this.item = item;

			CallNames = callNames;
			Name = name;
			Description = description;
			ExecuteDescription = executeDescription;
			Requirements = requirements;
			Effects = effects;
		}

		public void Execute() {
			Terminal.WriteLine(ExecuteDescription);
		}
	}
}