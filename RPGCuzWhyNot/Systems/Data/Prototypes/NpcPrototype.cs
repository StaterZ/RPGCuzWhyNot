using System;
using Newtonsoft.Json;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public class NpcPrototype : ThingPrototype {
		[JsonProperty("location", Required = Required.Always)]
		public string Location { get; set; }

		[JsonProperty("glanceDescription", Required = Required.Always)]
		public string GlanceDescription { get; set; }

		[JsonProperty("approachDescription", Required = Required.Always)]
		public string ApproachDescription { get; set; }
	}
}