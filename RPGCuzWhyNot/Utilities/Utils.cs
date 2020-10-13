using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RPGCuzWhyNot.Systems;

namespace RPGCuzWhyNot.Utilities {
	public static class Utils {
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
			Terminal.Write(question + "   " + Utils.StringifyArray("[", ", ", "]", options));

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

		public static string AddSignAndColor(int value, bool positiveIsGood = true, bool showPositiveSign = true) {
			if (value == 0)
				return "{yellow}(0)";

			bool valueIsPositive = value > 0;
			string color = (valueIsPositive == positiveIsGood) ? "green" : "red";

			string sign = value > 0 && showPositiveSign ? "+" : "";
			return $"{{{color}}}({sign}{value})";
		}

		public static string AddSignAndColor(float value, bool positiveIsGood = true, bool showPositiveSign = true) {
			if (value == 0)
				return "{yellow}(0)";

			bool valueIsPositive = value > 0;
			string color = (valueIsPositive == positiveIsGood) ? "green" : "red";

			string sign = value > 0 && showPositiveSign ? "+" : "";
			return $"{{{color}}}({sign}{value})";
		}

		public static void WaitForPlayer() {
			Terminal.WriteWithoutDelay("{fg:Blue}(Press any key)");
			Terminal.ReadKey(true);
		}

		public static float Clamp(float value, float min, float max) {
			return Math.Min(Math.Max(value, min), max);
		}

		public static int Clamp(int value, int min, int max) {
			return Math.Min(Math.Max(value, min), max);
		}

		public static int Mod(int a, int b) {
			if (b == 0) {
				return 0;
			}

			return a - b * (int)Math.Floor((float)a / b);
		}
		public static string StringifyArray(string start, string separator, string end, string[] array) {
			return StringifyArray(start, separator, end, array, 0, array.Length);
		}

		public static string StringifyArray(string start, string separator, string end, string[] array, Range range) {
			var (begin, count) = range.GetOffsetAndLength(array.Length);
			return StringifyArray(start, separator, end, array, begin, count);
		}

		public static string StringifyArray(string start, string separator, string end, string[] array, int begin, int count) {
			StringBuilder builder = new StringBuilder();

			builder.Append(start);
			for (int i = begin; i < begin + count; i++) {
				if (i != begin) {
					builder.Append(separator);
				}

				builder.Append(array[i]);
			}
			builder.Append(end);

			return builder.ToString();
		}

		public static string FancyBitFlagEnum<E>(this E e) where E : Enum => FancyBitFlagEnum(e, out _);

		public static string FancyBitFlagEnum<E>(this E e, out int count) where E : Enum {
			List<string> res = new List<string>();
			foreach (Enum r in Enum.GetValues(typeof(E))) {
				if (e.HasFlag(r)) {
					res.Add(r.ToString());
				}
			}
			count = res.Count;
			switch (res.Count) {
				case 0: return "";
				case 1: return res[0];
				case 2: return $"{res[0]} and {res[1]}";
				default:
					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < res.Count - 1; ++i) {
						sb.Append(res[i]);
						sb.Append(", ");
					}
					sb.Append("and ");
					sb.Append(res[^1]);
					return sb.ToString();
			}
		}
	}
}