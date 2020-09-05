using System;

namespace RPGCuzWhyNot {
	public struct FGColorScope : IDisposable {
		public FGColorScope(ConsoleColor color) {
			Terminal.PushForegroundColor(color);
		}

		public void Dispose() {
			Terminal.PopState();
		}
	}
}

