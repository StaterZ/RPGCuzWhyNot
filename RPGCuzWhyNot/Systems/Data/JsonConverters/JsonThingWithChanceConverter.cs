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

				reader.ReadChecked();
				if (reader.TokenType != JsonToken.String)
					throw reader.CreateException("Expected string.");
				string itemId = (string)reader.Value;

				reader.ReadChecked();
				if (reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.Float)
					throw reader.CreateException("Expected number.");
				object firstNumber = reader.Value;

				reader.ReadChecked();
				if (reader.TokenType == JsonToken.EndArray) {
					// [string, float]
					float chance = Math.Max(0, Convert.ToSingle(firstNumber));
					return new ThingWithChance(itemId, chance);
				}

				if (reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float) {
					// [string, int, int]
					object secondNumber = reader.Value;

					reader.ReadChecked();
					if (reader.TokenType != JsonToken.EndArray)
						throw reader.CreateException("Expected end of array.");

					int minCount = Math.Max(0, Convert.ToInt32(firstNumber));
					int maxCount = Math.Max(0, Convert.ToInt32(secondNumber));
					return new ThingWithChance(itemId, minCount, maxCount);
				}

				throw reader.CreateException("Expected number.");
			}

			throw reader.CreateException("Expected string or array.");
		}

		public override void WriteJson(JsonWriter writer, ThingWithChance value, JsonSerializer serializer) {
			throw new NotSupportedException();
		}
	}
}