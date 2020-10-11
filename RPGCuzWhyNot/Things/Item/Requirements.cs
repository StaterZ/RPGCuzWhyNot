using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RPGCuzWhyNot.Things.Item {
	[Serializable]
	public class Requirements {
		[JsonProperty("stats")]
		public Stats Stats { get; set; }

		[JsonProperty("items")]
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