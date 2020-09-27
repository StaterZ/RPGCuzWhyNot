using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Things.Characters.NPCs;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public class NpcPrototype : Prototype {
		[JsonProperty("location")]
		public string Location { get; set; }

		[JsonProperty("glanceDescription")]
		public string GlanceDescription { get; set; }

		[JsonProperty("approachDescription")]
		public string ApproachDescription { get; set; }
	}
}