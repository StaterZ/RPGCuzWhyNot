using System;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Inventory.Item {
	[Serializable]
	public class ItemAction : IPlannableAction {
		[JsonPropertyName("callNames")]
		public string[] CallNames { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

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

		public ItemAction(string[] callNames, string name, string description, string executeDescription, Requirements requirements, Effects effects) {
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