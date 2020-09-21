using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Systems.Data.JsonConverters {
	/// <summary>
	/// Converts either a single object or a list of objects into a list of objects.
	/// </summary>
	public class JsonMaybeListConverter : JsonConverterFactory {
		public override bool CanConvert(Type typeToConvert) {
			return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(List<>);
		}

		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
			return (JsonConverter)Activator.CreateInstance(
				typeof(Inner<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]),
				BindingFlags.Instance | BindingFlags.Public,
				null,
				Array.Empty<object>(),
				null);
		}

		private class Inner<T> : JsonConverter<List<T>> {
			public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<T>();

				if (reader.TokenType == JsonTokenType.StartObject) {
					list.Add(JsonSerializer.Deserialize<T>(ref reader, options));
				}
				else if (reader.TokenType == JsonTokenType.StartArray) {
					reader.Read();
					do {
						list.Add(JsonSerializer.Deserialize<T>(ref reader, options));
						reader.Read();
					} while (reader.TokenType != JsonTokenType.EndArray);
				}
				else {
					throw new JsonException("Expected object or array.");
				}

				return list;
			}

			public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options) {
				throw new NotImplementedException();
			}
		}
	}
}