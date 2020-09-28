using Newtonsoft.Json;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	public abstract class Prototype {
		[JsonIgnore]
		public string DataFilePath { get; set; }

		[JsonProperty("id", Required = Required.Always)]
		public string Id { get; set; }

		[JsonProperty("callName", Required = Required.Always)]
		public string CallName { get; set; }

		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }
	}
}