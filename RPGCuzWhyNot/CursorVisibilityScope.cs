using System;

namespace RPGCuzWhyNot
{
	public class CursorVisibilityScope : IDisposable {
		private readonly bool prevIsVisible;

		public CursorVisibilityScope(bool isVisible) {
			prevIsVisible = Console.CursorVisible;
			Console.CursorVisible = isVisible;
		}

		public void Dispose() {
			Console.CursorVisible = prevIsVisible;
		}
	}
}