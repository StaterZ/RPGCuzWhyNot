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
			return Terminal.ReadLine();
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
				string answer = Terminal.ReadLine();

				if (options.Contains(answer)) {
					return answer;
				}

				Terminal.WriteLine("Invalid answer");
			}
		}

		public static void PrintDivider(char c) {
			Terminal.WriteLine(new string(c, Terminal.WindowSize.x));
		}

		private static readonly char[] wheelLookup = { '/', '-', '\\', '|' };
		public static void FakeLoad(int millis) {
			Vec2 startPos = Terminal.CursorPosition;

			Terminal.PushState(Terminal.Save.ForegroundColor);
			Terminal.ForegroundColor = ConsoleColor.Yellow;
			Terminal.Write("Fetching... ");

			Vec2 loadPos = Terminal.CursorPosition;
			for (int i = 0; i < millis / 100; i++) {
				Terminal.Write(wheelLookup[i % wheelLookup.Length]);
				Terminal.CursorPosition = loadPos;
				Thread.Sleep(100);
			}
			Terminal.PopState();

			Terminal.CursorPosition = startPos;
			Terminal.WriteLine("{fg:Green}(Loading... Done!)");
			Terminal.Beep(1000, 100);
		}

		public static void Sleep(int millis) {
			Thread.Sleep(millis);
		}
	}
}