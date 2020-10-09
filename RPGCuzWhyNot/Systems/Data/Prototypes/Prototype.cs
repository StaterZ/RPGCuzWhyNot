using System;
using Newtonsoft.Json;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	public abstract class Prototype {
		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }

		[JsonIgnore]
		public string DataFilePath { get; set; }
	}
}