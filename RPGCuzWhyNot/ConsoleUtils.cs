using System;
using System.Linq;
using System.Text;

namespace RPGCuzWhyNot {
	public static class ConsoleUtils {
		/// <summary>
		/// asks a qusetion to the player
		/// </summary>
		/// <param name="question">the question to display to the player</param>
		/// <returns></returns>
		public static string Ask(string question) {
			Console.Write(question);
			return Console.ReadLine();
		}

		/// <summary>
		/// asks a qusetion to the player but limit what they can respond with
		/// </summary>
		/// <param name="question">the question to display to the player</param>
		/// <param name="options">the valid things the player can respond with</param>
		/// <returns></returns>
		public static string Ask(string question, params string[] options) {
			Console.Write(question + "   " + StringifyArray("[", ", ", "]", options));

			string answer;
			while (true) {
				answer = Console.ReadLine();

				if (options.Contains(answer)) {
					return answer;
                }

				Console.WriteLine("Invalid answer");
			}
		}

		private static string StringifyArray(string start, string separator, string end, string[] array) {
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
	}
}