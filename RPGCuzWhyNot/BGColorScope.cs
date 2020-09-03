using System;

namespace RPGCuzWhyNot {
	public class BGColorScope : IDisposable {
		public BGColorScope(ConsoleColor color) {
			Terminal.PushForegroundColor(color);
		}

		public void Dispose() {
			Terminal.PopState();
		}
	}
}

