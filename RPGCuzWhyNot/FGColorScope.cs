using System;

namespace RPGCuzWhyNot {
	public class FGColorScope : IDisposable {
		private readonly ConsoleColor prevColor;

		public FGColorScope(ConsoleColor color) {
			prevColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
		}

		public void Dispose() {
			Console.ForegroundColor = prevColor;
		}
	}
}