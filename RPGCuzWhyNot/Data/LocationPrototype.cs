using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Data {
	[Serializable]
	public class LocationPrototype {
		[JsonPropertyName("callName")]
		public string CallName { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("items")]
		public List<string> Items { get; set; } = new List<string>();

		[JsonPropertyName("paths")]
		public Dictionary<string, string> Paths { get; set; } = new Dictionary<string, string>();

		public Location Create() {
			return new Location(CallName, Name, Description);
		}
	}
}