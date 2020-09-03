using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Data {
	[Serializable]
	public class LocationPrototype : Prototype {
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("pathDescription")]
		public string PathDescription { get; set; }

		[JsonPropertyName("items")]
		public List<string> Items { get; set; } = new List<string>();

		[JsonPropertyName("paths")]
		public List<string> Paths { get; set; } = new List<string>();

		public Location Instantiate() {
			return new Location(CallName, Name, Description, PathDescription);
		}
	}
}