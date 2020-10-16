using System;
using System.Runtime.InteropServices;
using System.Text;
using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.VirtualTerminal {
	/// <summary>
	/// Abstraction for working with virtual terminal control sequences.
	/// </summary>
	public static class VT {
		// Resources:
		//   https://docs.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
		//   https://en.wikipedia.org/wiki/ANSI_escape_code
		//   https://tintin.mudhalla.net/info/ansicolor/
		//   https://www.vt100.net/docs/vt100-ug/contents.html

		// Control Sequence Indicator
		private const string CSI = "\x1b[";

		private static readonly StringBuilder sb = new StringBuilder();


		public static void SelectGraphicRendition(GraphicsRendition value) {
			Write(CSI);
			Write((byte)value);
			Write('m');
		}

		public static void Color256(ColorLayer layer, byte value) {
			Write(layer == ColorLayer.Foreground ? CSI + "38;5;" : CSI + "48;5;");
			Write(value);
			Write('m');
		}

		public static void TrueColor(ColorLayer layer, byte r, byte g, byte b) {
			sb.Clear();
			sb.Append(layer == ColorLayer.Foreground ? CSI + "38;2;" : CSI + "48;2;");
			sb.Append(r);
			sb.Append(';');
			sb.Append(g);
			sb.Append(';');
			sb.Append(b);
			sb.Append('m');
			Write(sb.ToString());
		}

		public static void ResetColor() => Write(CSI + "0m");

		public static void ClearLine() => Write(CSI + "K");

		public static void SetScreenBuffer(ScreenBuffer buffer) {
			Write(buffer == ScreenBuffer.Alternate ? CSI + "?1049h" : CSI + "?1049l");
		}


		private static void Write(string str) => Console.Write(str);

		private static void Write(char chr) => Console.Write(chr);

		private static void Write(byte num) => Console.Write(num.ToString());


		#region Platform specific

		/// <summary>
		/// Enables virtual terminal processing (only has an effect on Windows).
		/// </summary>
		public static void EnableVirtualTerminalProcessing() {
			if (Environment.OSVersion.Platform != PlatformID.Win32NT) return;

			IntPtr consoleHandle = GetStdHandle(-11); // STD_OUTPUT_HANDLE
			if (consoleHandle == (IntPtr)(-1)) return;
			if (!GetConsoleMode(consoleHandle, out int consoleMode)) return;
			SetConsoleMode(consoleHandle, consoleMode | 0x4); // ENABLE_VIRTUAL_TERMINAL_PROCESSING
		}

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll")]
		private static extern bool GetConsoleMode(
			IntPtr hConsoleHandle,
			out int lpMode
		);

		[DllImport("kernel32.dll")]
		private static extern bool SetConsoleMode(
			IntPtr hConsoleHandle,
			int dwMode
		);

		#endregion
	}
}