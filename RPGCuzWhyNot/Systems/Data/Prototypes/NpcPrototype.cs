using System;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Things.Characters.NPCs;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	public class NpcPrototype : Prototype {
		[JsonPropertyName("location")]
		public string Location { get; set; }

		[JsonPropertyName("glanceDescription")]
		public string GlanceDescription { get; set; }

		[JsonPropertyName("approachDescription")]
		public string ApproachDescription { get; set; }
	}
}