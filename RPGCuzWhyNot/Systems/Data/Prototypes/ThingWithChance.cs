using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Systems.Data.JsonConverters;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable, JsonConverter(typeof(JsonThingWithChanceConverter))]
	public class ThingWithChance {
		public string Id { get; set; }
		public float Chance { get; set; }
		public int MinCount { get; set; }
		public int MaxCount { get; set; }
		public bool IsRange { get; set; }

		public ThingWithChance(string id, float chance) {
			Id = id;
			Chance = chance;
		}

		public ThingWithChance(string id, int minCount, int maxCount) {
			Id = id;
			MinCount = minCount;
			MaxCount = maxCount;
			IsRange = true;
		}

		public int EvaluateChance(Random random) {
			if (IsRange)
				return random.Next(MinCount, MaxCount + 1);

			int count = (int)Chance;
			if (count == Chance)
				return count;

			if ((float)random.NextDouble() < Chance - count)
				count += 1;

			return count;
		}
	}
}