using Newtonsoft.Json;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	public abstract class Prototype {
		[JsonIgnore]
		public string DataFilePath { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("callName")]
		public string CallName { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}
}