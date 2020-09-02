using System;

namespace RPGCuzWhyNot {
	public class BGColorScope : IDisposable {
		private readonly ConsoleColor prevColor;

		public BGColorScope(ConsoleColor color) {
			prevColor = Console.BackgroundColor;
			Console.BackgroundColor = color;
		}

		public void Dispose() {
			Console.BackgroundColor = prevColor;
		}
	}
}