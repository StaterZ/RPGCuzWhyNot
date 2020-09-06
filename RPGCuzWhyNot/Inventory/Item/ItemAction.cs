using System;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Inventory.Item {
	[Serializable]
	public class ItemAction {
		[JsonPropertyName("callNames")]
		public string[] CallNames { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("requirements")]
		public Requirements Requirements { get; set; }

		[JsonPropertyName("effects")]
		public Effects Effects { get; set; }

		public ItemAction() {
			Requirements = new Requirements();
			Effects = new Effects();
		}

		public ItemAction(string[] callNames, string name, string description, Requirements requirements, Effects effects) {
			CallNames = callNames;
			Name = name;
			Description = description;
			Requirements = requirements;
			Effects = effects;
		}

		public void Execute() {
			Terminal.WriteLine("me did le execute");
		}
	}
}