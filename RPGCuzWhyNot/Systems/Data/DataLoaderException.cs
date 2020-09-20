using System;

namespace RPGCuzWhyNot.Systems.Data {
	public class DataLoaderException : Exception {
		public DataLoaderException() { }
		public DataLoaderException(string message) : base(message) { }
		public DataLoaderException(string message, Exception innerException) : base(message, innerException) { }
	}
}