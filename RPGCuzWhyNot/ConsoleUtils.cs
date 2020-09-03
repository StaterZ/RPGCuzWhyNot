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
			Terminal.Write(question);
			return Console.ReadLine();
		}

		/// <summary>
		/// Asks a question to the player, but limits what they can respond with.
		/// </summary>
		/// <param name="question">The question to display to the player.</param>
		/// <param name="options">The options the player can respond with.</param>
		/// <returns></returns>
		public static string Ask(string question, params string[] options) {
			Terminal.Write(question + "   " + StringifyArray("[", ", ", "]", options));

			while (true) {
				string answer = Console.ReadLine();

				if (options.Contains(answer)) {
					return answer;
				}

				Terminal.WriteLine("Invalid answer");
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

		public static void PrintDivider(char c) {
			Terminal.WriteLine(new string(c, Console.WindowWidth) + "\n");
		}

		private static readonly char[] wheelLookup = { '/', '-', '\\', '|' };
		public static void FakeLoad(int millis) {
			ConsoleColor fg = Console.ForegroundColor;

			int startX = Console.CursorLeft;
			int startY = Console.CursorTop;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Fetching... ");

			int x = Console.CursorLeft;
			int y = Console.CursorTop;

			Console.CursorVisible = false;
			for (int i = 0; i < millis / 100; i++) {
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