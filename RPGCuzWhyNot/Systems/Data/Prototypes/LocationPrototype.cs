using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public class LocationPrototype : ThingPrototype {
		[JsonProperty("description", Required = Required.Always)]
		public string Description { get; set; }

		[JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
		public List<string> Items { get; set; } = new List<string>();

		[JsonProperty("paths", NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, string> Paths { get; set; } = new Dictionary<string, string>();

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