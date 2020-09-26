using System;
using System.Linq;
using System.Threading;
using RPGCuzWhyNot.Systems;

namespace RPGCuzWhyNot.Utilities {
	public static class ConsoleUtils {
		/// <summary>
		/// Asks a question to the player.
		/// </summary>
		/// <param name="question">The question to display to the player.</param>
		/// <returns></returns>
		public static string Ask(string question) {
			Terminal.Write(question);
			using (Terminal.PushState()) {
				Terminal.IsCursorVisible = true;
				return Terminal.ReadLine();
			}
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
				string answer;
				using (Terminal.PushState()) {
					Terminal.IsCursorVisible = true;
					answer = Terminal.ReadLine();
				}

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

		public static string FormatInt(int value) {
			if (value > 0) {
				return $"{{fg:Green}}(+{value})";
			} else if (value < 0) {
				return $"{{fg:Red}}({value})";
			} else {
				return $"{{fg:Yellow}}({value})";
			}
		}

		public static void WaitForPlayer() {
			Terminal.WriteDirect("Press any key to continue");
			Console.ReadKey(true);
		}
	}
}