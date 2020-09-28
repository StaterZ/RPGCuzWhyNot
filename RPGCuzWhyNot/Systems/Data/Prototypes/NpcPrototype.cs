using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Things.Characters.NPCs;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public class NpcPrototype : Prototype {
		[JsonProperty("location", Required = Required.Always)]
		public string Location { get; set; }

		[JsonProperty("glanceDescription", Required = Required.Always)]
		public string GlanceDescription { get; set; }

		[JsonProperty("approachDescription", Required = Required.Always)]
		public string ApproachDescription { get; set; }
	}
}