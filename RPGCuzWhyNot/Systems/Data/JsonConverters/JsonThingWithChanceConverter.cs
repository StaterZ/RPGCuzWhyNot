using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Systems.Data.Prototypes;

namespace RPGCuzWhyNot.Systems.Data.JsonConverters {
	public class JsonThingWithChanceConverter : JsonConverter<ThingWithChance> {
		public override ThingWithChance ReadJson(JsonReader reader, Type objectType, ThingWithChance existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (reader.TokenType == JsonToken.String) {
				// string
				return new ThingWithChance((string)reader.Value, 1f);
			}

			if (reader.TokenType == JsonToken.StartArray) {
				// [string, float] or [string, int, int]

				reader.Read();
				if (reader.TokenType != JsonToken.String)
					throw new JsonException("Expected string.");
				string itemId = (string)reader.Value;

				reader.Read();
				if (reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.Float)
					throw new JsonException("Expected number.");
				object firstNumber = reader.Value;

				reader.Read();
				if (reader.TokenType == JsonToken.EndArray) {
					// [string, float]
					float chance = Convert.ToSingle(firstNumber);
					return new ThingWithChance(itemId, chance);
				}

				if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float) {
					object secondNumber = reader.Value;

					reader.Read();
					if (reader.TokenType != JsonToken.EndArray)
						throw new JsonException("Expected end of array.");

					int minCount = Convert.ToInt32(firstNumber);
					int maxCount = Convert.ToInt32(secondNumber);
					return new ThingWithChance(itemId, minCount, maxCount);
				}

				throw new JsonException("Expected number.");
			}

			throw new JsonException("Expected string or array.");
		}

		public override void WriteJson(JsonWriter writer, ThingWithChance value, JsonSerializer serializer) {
			throw new NotSupportedException();
		}
	}
}