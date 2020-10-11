using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Things.Item {
	[Serializable]
	public class Requirements {
		[JsonPropertyName("stats")]
		public Stats Stats { get; set; }

		[JsonPropertyName("items")]
		public Dictionary<string, int> Items { get; set; }

		public Requirements() {
			Stats = new Stats();
			Items = new Dictionary<string, int>();
		}

		public Requirements(Stats stats, Dictionary<string, int> items) {
			Stats = stats;
			Items = items;
		}
	}
}