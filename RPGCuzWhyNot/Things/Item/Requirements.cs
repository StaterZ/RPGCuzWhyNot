using System;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Things.Item {
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