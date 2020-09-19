using System;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Inventory.Item {
	[Serializable]
	public class Requirements {
		[JsonPropertyName("stats")]
		public Stats Stats { get; set; }

		public Requirements() {
			Stats = new Stats();
		}

		public Requirements(Stats stats) {
			Stats = stats;
		}
	}
}