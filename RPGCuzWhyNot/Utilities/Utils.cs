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
			Terminal.Write(question + "   " + options.Stringify("[", ", ", "]"));

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
			if (value == 0) {
				return "{yellow}(0)";
			}

			bool valueIsPositive = value > 0;
			string color = (valueIsPositive == positiveIsGood) ? "green" : "red";

			string sign = valueIsPositive && showPositiveSign ? "+" : "";
			return $"{{{color}}}({sign}{value})";
		}

		public static string AddSignAndColor(float value, bool positiveIsGood = true, bool showPositiveSign = true) {
			if (value == 0) {
				return "{yellow}(0)";
			}

			bool valueIsPositive = value > 0;
			string color = (valueIsPositive == positiveIsGood) ? "green" : "red";

			string sign = valueIsPositive && showPositiveSign ? "+" : "";
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

			return a - b * (int) Math.Floor((float) a / b);
		}

		public static string Stringify<T>(this T[] array, string start, Func<int, int, string> separatorFunc, string end, Range range) {
			StringBuilder builder = new StringBuilder();

			builder.Append(start);
			(int begin, int count) = range.GetOffsetAndLength(array.Length);
			for (int i = 0; i < count; i++) {
				if (i > 0) {
					builder.Append(separatorFunc(i - 1, count - 1)); //separator index and count, not item index and count
				}

				builder.Append(array[begin + i]);
			}

			builder.Append(end);

			return builder.ToString();
		}

		public static string Stringify<T>(this T[] array, string start, Func<int, int, string> separatorFunc, string end) {
			return array.Stringify(start, separatorFunc, end, Range.All);
		}

		public static string Stringify<T>(this T[] array, string start, string separator, string end, Range range) {
			return array.Stringify(start, (i, l) => separator, end, range);
		}

		public static string Stringify<T>(this T[] array, string start, string separator, string end) {
			return array.Stringify(start, separator, end, Range.All);
		}

		public static string Stringify<T>(this T[] array, string start, string leadingSeparartor, string middleSeparator, string trailingSeparator, string end, Range range) {
			return array.Stringify(start, (i, l) => {
				if (i == 0) {
					return leadingSeparartor;
				} else if (i == l - 1) {
					return trailingSeparator;
				} else {
					return middleSeparator;
				}
			}, end, range);
		}

		public static string Stringify<T>(this T[] array, string start, string leadingSeparartor, string middleSeparator, string trailingSeparator, string end) {
			return array.Stringify(start, leadingSeparartor, middleSeparator, trailingSeparator, end, Range.All);
		}

		public static IEnumerable<E> GetBitFlags<E>(this E e) where E : Enum {
			return Enum.GetValues(typeof(E)).Cast<E>().Where(value => e.HasFlag(value));
		}

		public static string FancyBitFlagEnum<E>(this E e) where E : Enum => FancyBitFlagEnum(e, out _);

		public static string FancyBitFlagEnum<E>(this E e, out int count) where E : Enum {
			E[] bitflags = e.GetBitFlags().ToArray();
			count = bitflags.Length;
			return bitflags.Stringify("", ", ", ", ", " and ", "");
		}
	}
}