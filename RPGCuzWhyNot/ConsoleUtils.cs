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
			Terminal.Write(question + "   " + Stringification.StringifyArray("[", ", ", "]", options));

			while (true) {
				string answer = Console.ReadLine();

				if (options.Contains(answer)) {
					return answer;
				}

				Terminal.WriteLine("Invalid answer");
			}
		}

		public static void PrintDivider(char c) {
			Terminal.WriteLine(new string(c, Console.WindowWidth) + "\n");
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
						Terminal.Write(wheelLookup[i % wheelLookup.Length]);
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