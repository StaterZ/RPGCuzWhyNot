using Newtonsoft.Json;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	public abstract class ThingPrototype : Prototype {
		[JsonProperty("callName", Required = Required.Always)]
		public string CallName { get; set; }
	}
}