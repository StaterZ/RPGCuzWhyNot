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
					throw CreateException(reader, "Expected string.");
				string itemId = (string)reader.Value;

				reader.Read();
				if (reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.Float)
					throw CreateException(reader, "Expected number.");
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
						throw CreateException(reader, "Expected end of array.");

					int minCount = Convert.ToInt32(firstNumber);
					int maxCount = Convert.ToInt32(secondNumber);
					return new ThingWithChance(itemId, minCount, maxCount);
				}

				throw CreateException(reader, "Expected number.");
			}

			throw CreateException(reader, "Expected string or array.");
		}

		public override void WriteJson(JsonWriter writer, ThingWithChance value, JsonSerializer serializer) {
			throw new NotSupportedException();
		}

		private static JsonReaderException CreateException(JsonReader reader, string message) {
			var lineInfo = (IJsonLineInfo)reader;

			return new JsonReaderException(
				$"{message} Path '{reader.Path}', line {lineInfo.LineNumber}, position {lineInfo.LinePosition}.",
				reader.Path, lineInfo.LineNumber, lineInfo.LinePosition, null);
		}
	}
}