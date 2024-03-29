using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.Data.JsonConverters {
	public class JsonFractionConverter : JsonConverter<Fraction> {
		private static int ParseInt(JsonReader reader, ReadOnlySpan<char> str) {
			if (!int.TryParse(str, out int value))
				throw reader.CreateException("Bad integer in fraction.");

			return value;
		}

		public override Fraction ReadJson(JsonReader reader, Type objectType, Fraction existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (reader.TokenType != JsonToken.String)
				throw new JsonException("Fraction string expected.");

			string str = (string)reader.Value;

			int divIndex = str!.IndexOf('/');
			if (divIndex != -1) {
				return new Fraction(
					ParseInt(reader, str.AsSpan(0, divIndex)),
					ParseInt(reader, str.AsSpan(divIndex + 1)));
			}

			return new Fraction(ParseInt(reader, str), 1);
		}

		public override void WriteJson(JsonWriter writer, Fraction value, JsonSerializer serializer) {
			writer.WriteValue(value.denominator == 1
				                  ? value.numerator.ToString()
				                  : $"{value.numerator}/{value.denominator}");
		}
	}
}