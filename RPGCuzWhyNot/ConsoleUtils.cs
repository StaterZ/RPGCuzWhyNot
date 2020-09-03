using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RPGCuzWhyNot {
	public static class ConsoleUtils {
		/// <summary>
		/// Asks a question to the player.
		/// </summary>
		/// <param name="question">The question to display to the player.</param>
		/// <returns></returns>
		public static string Ask(string question) {
			SlowWrite(question);
			return Console.ReadLine();
		}

		/// <summary>
		/// Asks a question to the player, but limits what they can respond with.
		/// </summary>
		/// <param name="question">The question to display to the player.</param>
		/// <param name="options">The options the player can respond with.</param>
		/// <returns></returns>
		public static string Ask(string question, params string[] options) {
			SlowWrite(question + "   " + StringifyArray("[", ", ", "]", options));

			while (true) {
				string answer = Console.ReadLine();

				if (options.Contains(answer)) {
					return answer;
				}

				SlowWriteLine("Invalid answer");
			}
		}

		public static string StringifyArray(string start, string separator, string end, string[] array) {
			StringBuilder builder = new StringBuilder();

			builder.Append(start);
			for (int i = 0; i < array.Length; i++) {
				if (i > 0) {
					builder.Append(separator);
				}

				builder.Append(array[i]);
			}
			builder.Append(end);

			return builder.ToString();
		}

		public static void SlowWriteLine(string text, int beepFrequency = 200, int charsPerSec = 100) {
			SlowWrite(text + '\n', beepFrequency, charsPerSec);
		}

		private static int charsBeepCounter;
		public static void SlowWrite(string text, int beepFrequency = 200, int charsPerSec = 100) {
			using (new CursorVisibilityScope(false)) {
				const int charsPerBeep = 15;
				int millisPerChar = 1000 / charsPerSec;

				foreach (char c in text) {
					SmartWrite(c);
					charsBeepCounter++;

					if (charsBeepCounter >= charsPerBeep) {
						charsBeepCounter = 0;
						Console.Beep(beepFrequency, millisPerChar);
					} else {
						Thread.Sleep(millisPerChar);
					}
				}
			}
		}

		private static readonly Stack<(ColorScope scope, ColorState state)> colorStack = new Stack<(ColorScope scope, ColorState state)>();

		public static void SmartWrite(char c) {
			ColorScope stop;
			if (colorStack.Count > 0 && c == colorStack.Peek().scope.stop) {
				stop = colorStack.Peek().scope;
			}else{
				stop = null;
			}

			ColorScope start = null;
			foreach (ColorScope colorScope in colorScopes) {
				if (c == colorScope.start) {
					start = colorScope;
					break;
				}
			}

			void Start() {
				colorStack.Push((start, new ColorState(Console.ForegroundColor, Console.BackgroundColor)));

				if (start.fg.HasValue) {
					Console.ForegroundColor = start.fg.Value;
				}
				if (start.bg.HasValue) {
					Console.BackgroundColor = start.bg.Value;
				}
			}

			void Stop() {
				(ColorScope scope, ColorState state) prev = colorStack.Pop();
				
				Console.ForegroundColor = prev.state.fg;
				Console.BackgroundColor = prev.state.bg;
			}

			if (stop != null && !stop.includeStart) {
				Stop();
			}
			if (start != null && start.includeStart) {
				Start();
			}

			Console.Write(c);

			if (start != null && !start.includeStart) {
				Start();
			}

			if (stop != null && stop.includeStart) {
				Stop();
			}
		}

		public static readonly List<ColorScope> colorScopes = new List<ColorScope>();

		public class ColorScope {
			public readonly char start;
			public readonly char stop;
			public readonly bool includeStart;
			public readonly bool includeStop;
			public readonly ConsoleColor? fg;
			public readonly ConsoleColor? bg;

			public ColorScope(char start, char stop, bool includeStart, bool includeStop, ConsoleColor? fg, ConsoleColor? bg = null) {
				this.start = start;
				this.stop = stop;
				this.includeStart = includeStart;
				this.includeStop = includeStop;
				this.fg = fg;
				this.bg = bg;
			}
		}

		public class ColorState {
			public readonly ConsoleColor fg;
			public readonly ConsoleColor bg;

			public ColorState(ConsoleColor fg, ConsoleColor bg) {
				this.fg = fg;
				this.bg = bg;
			}
		}

		public static void PrintDivider(char c) {
			SlowWriteLine(new string(c, Console.WindowWidth) + "\n");
		}

		private static readonly char[] wheelLookup = { '/', '-', '\\', '|' };
		public static void FakeLoad(int millis) {
			using (new CursorVisibilityScope(false)) {
				int startX = Console.CursorLeft;
				int startY = Console.CursorTop;

				using (new FGColorScope(ConsoleColor.Yellow)) {
					Console.Write("Fetching... ");

					int x = Console.CursorLeft;
					int y = Console.CursorTop;

					for (int i = 0; i < millis / 100; i++)
					{
						SmartWrite(wheelLookup[i % wheelLookup.Length]);
						Console.CursorLeft = x;
						Console.CursorTop = y;
						Thread.Sleep(100);
					}
				}

				Console.CursorLeft = startX;
				Console.CursorTop = startY;
				using (new FGColorScope(ConsoleColor.Green)) {
					Console.WriteLine("Loading... Done!");
				}
				Console.Beep(1000, 100);
			}
		}

		public static void Sleep(int millis) {
			Thread.Sleep(millis);
		}
	}
}