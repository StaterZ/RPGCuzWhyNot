using System;
using Newtonsoft.Json;

namespace RPGCuzWhyNot.Systems.Data {
	public static class JsonReaderExtensions {
		/// <summary>
		/// Creates a <see cref="JsonReaderException"/> with additional line information in the message.
		/// </summary>
		public static JsonReaderException CreateException(this JsonReader reader, string message, Exception innerException = null) {
			var lineInfo = (IJsonLineInfo)reader;
			return new JsonReaderException(
				$"{message} Path '{reader.Path}', line {lineInfo.LineNumber}, position {lineInfo.LinePosition}.",
				reader.Path, lineInfo.LineNumber, lineInfo.LinePosition, innerException);
		}

		/// <summary>
		/// Reads the next JSON token from the source, throwing an exception on failure.
		/// </summary>
		/// <exception cref="JsonReaderException"></exception>
		public static void ReadChecked(this JsonReader reader) {
			if (!reader.Read())
				throw reader.CreateException("Unexpected end of file.");
		}
	}
}