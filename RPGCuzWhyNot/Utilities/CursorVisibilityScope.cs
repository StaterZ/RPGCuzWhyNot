using RPGCuzWhyNot.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPGCuzWhyNot.Utilities {
	public struct CursorVisibilityScope : IDisposable {
		private readonly bool prevIsCursorVisible;

		public CursorVisibilityScope(bool isCursorVisible) {
			prevIsCursorVisible = Terminal.IsCursorVisible;
			Terminal.IsCursorVisible = isCursorVisible;
		}

		public void Dispose() {
			Terminal.IsCursorVisible = prevIsCursorVisible;
		}
	}
}
