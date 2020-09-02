﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RPGCuzWhyNot {
	public static class ConsoleUtils {
		/// <summary>
		/// asks a qusetion to the player
		/// </summary>
		/// <param name="question">the question to display to the player</param>
		/// <returns></returns>
		public static string Ask(string question) {
			SlowWrite(question);
			return Console.ReadLine();
		}

		/// <summary>
		/// asks a qusetion to the player but limit what they can respond with
		/// </summary>
		/// <param name="question">the question to display to the player</param>
		/// <param name="options">the valid things the player can respond with</param>
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

		public static void SlowWriteLine(string text, int frequensy = 200, float charsPerSec = 100) {
			SlowWrite(text + '\n', frequensy, charsPerSec);
		}

		private static int charsPrinted;
		public static void SlowWrite(string text, int frequensy = 200, float charsPerSec = 100) {
			const int charsPerBeep = 15;
			int milisPerChar = (int)Math.Round(1 / charsPerSec * 1000);

			foreach (char c in text) {

				SmartWrite(c);
				charsPrinted++;

				if (charsPrinted % charsPerBeep == 0) {
					Console.Beep(frequensy, milisPerChar);
				} else {
					Thread.Sleep(milisPerChar);
				}
			}
		}

		private static readonly Stack<ColorState> colorStates = new Stack<ColorState>();

		public static void SmartWrite(char c) {
			ColorScope stop = null;
			ColorScope start = null;
			foreach (ColorScope colorScope in colorScopes) {
				if (c == colorScope.stop) {
					stop = colorScope;
					break;
				}
				if (c == colorScope.start) {
					start = colorScope;
					break;
				}
			}

			void Start() {
				colorStates.Push(new ColorState(Console.ForegroundColor, Console.BackgroundColor));

				if (start.fg.HasValue) {
					Console.ForegroundColor = start.fg.Value;
				}
				if (start.bg.HasValue) {
					Console.BackgroundColor = start.bg.Value;
				}
			}

			void Stop() {
				ColorState prev = colorStates.Pop();
				
				Console.ForegroundColor = prev.fg;
				Console.BackgroundColor = prev.bg;
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

		public static void PrintDivider(char c, ConsoleColor color) {
			ConsoleColor fg = Console.ForegroundColor;

			Console.ForegroundColor = color;
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < Console.WindowWidth; i++) {
				builder.Append(c);
			}
			builder.AppendLine();
			SlowWriteLine(builder.ToString());

			Console.ForegroundColor = fg;
		}

		private static readonly char[] wheelLookup = { '/', '-', '\\', '|' };
		public static void FakeLoad(int milis) {
			ConsoleColor fg = Console.ForegroundColor;

			int startX = Console.CursorLeft;
			int startY = Console.CursorTop;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Fetching... ");

			int x = Console.CursorLeft;
			int y = Console.CursorTop;

			Console.CursorVisible = false;
			for (int i = 0; i < milis / 100; i++) {
				Console.Write(wheelLookup[i % wheelLookup.Length]);
				Console.CursorLeft = x;
				Console.CursorTop = y;
				Thread.Sleep(100);
			}

			Console.CursorVisible = true;

			Console.ForegroundColor = ConsoleColor.Green;
			Console.CursorLeft = startX;
			Console.CursorTop = startY;
			Console.WriteLine("Loading... Done!");
			Console.Beep(1000, 100);

			Console.ForegroundColor = fg;
		}
	}
}