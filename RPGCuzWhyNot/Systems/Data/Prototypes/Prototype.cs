using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	public abstract class Prototype : IOnDeserialized {
		[JsonIgnore]
		public string DataFilePath { get; set; }

		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("callName")]
		public string CallName { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		void IOnDeserialized.OnDeserialized() {
			OnDeserialized();
		}

		protected virtual void OnDeserialized() { }
	}
}