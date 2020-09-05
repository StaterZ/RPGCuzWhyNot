using System;

namespace RPGCuzWhyNot.Data {
	public class DataLoaderException : Exception {
		public DataLoaderException() { }
		public DataLoaderException(string message) : base(message) { }
		public DataLoaderException(string message, Exception innerException) : base(message, innerException) { }
	}
}