using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.Data {
	[Serializable]
	public class LocationPrototype : Prototype {
		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("items")]
		public List<string> Items { get; set; } = new List<string>();

		[JsonPropertyName("paths")]
		public Dictionary<string, string> Paths { get; set; } = new Dictionary<string, string>();

		protected override void OnDeserialized() {
			Items ??= new List<string>();
			Paths ??= new Dictionary<string, string>();
		}

		/// <summary>
		/// Create an instance of the prototype.
		/// </summary>
		public Location Create() {
			return new Location(CallName, Name, Description) {
				Prototype = this
			};
		}
	}
}