using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.Data {
	public class JsonFractionConverter : JsonConverterFactory {
		public override bool CanConvert(Type typeToConvert) {
			return typeToConvert == typeof(Fraction);
		}

		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
			return new Inner();
		}

		private class Inner : JsonConverter<Fraction> {
			private static int ParseInt(ReadOnlySpan<char> str) {
				if (!int.TryParse(str, out int value))
					throw new JsonException();

				return value;
			}

			public override Fraction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				if (reader.TokenType != JsonTokenType.String)
					throw new JsonException();

				string str = reader.GetString();

				int divIndex = str.IndexOf('/');
				if (divIndex != -1) {
					return new Fraction(
						ParseInt(str.AsSpan(0, divIndex)),
						ParseInt(str.AsSpan(divIndex + 1)));
				}

				return new Fraction(ParseInt(str), 1);
			}

			public override void Write(Utf8JsonWriter writer, Fraction value, JsonSerializerOptions options) {
				writer.WriteStringValue(value.denominator == 1
					                        ? value.numerator.ToString()
					                        : $"{value.numerator}/{value.denominator}");
			}
		}
	}
}