using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Systems.Data.JsonConverters {
	public class JsonEnumConverter : JsonConverterFactory {
		public override bool CanConvert(Type typeToConvert) {
			return typeToConvert.IsEnum;
		}

		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
			return (JsonConverter)Activator.CreateInstance(
				typeof(JsonEnumConverterInner<>).MakeGenericType(typeToConvert),
				BindingFlags.Instance | BindingFlags.Public,
				null,
				Array.Empty<object>(),
				null);
		}

		private class JsonEnumConverterInner<T> : JsonConverter<T> where T : struct, Enum {
			public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				Debug.Assert(typeToConvert == typeof(T));
				string valueString = reader.GetString();
				return Enum.Parse<T>(valueString, true);
			}

			public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
				writer.WriteStringValue(value.ToString());
			}
		}
	}
}