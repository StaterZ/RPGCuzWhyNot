using System;
using System.Collections.Generic;
using System.Text;

namespace RPGCuzWhyNot {
	public static class Stringification {
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
