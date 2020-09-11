using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Systems.Data {
	public abstract class Prototype : IOnDeserialized {
		[JsonIgnore]
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