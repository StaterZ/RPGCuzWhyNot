using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Data {
	public abstract class Prototype {
		[JsonIgnore]
		public string Id { get; set; }

		[JsonPropertyName("callName")]
		public string CallName { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}