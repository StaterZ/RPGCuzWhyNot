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

		public static void Sleep(int millis) {
			Thread.Sleep(millis);
		}
	}
}