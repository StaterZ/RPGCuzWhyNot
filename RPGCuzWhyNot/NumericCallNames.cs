using System.Collections.Generic;

namespace RPGCuzWhyNot {
	public static class NumericCallNames {
		private static readonly List<IThing> things = new List<IThing>();

		public static bool Get<T>(string index, out T thing) where T : IThing {
			thing = default;
			return int.TryParse(index, out int i) && Get(i, out thing);
		}

		public static bool Get<T>(int index, out T thing) where T : IThing {
			if (Get(index, out IThing t) && t is T tt) {
				thing = tt;
				return true;
			}
			thing = default;
			return false;
		}

		public static bool Get(string index, out IThing thing) {
			thing = default;
			return int.TryParse(index, out int i) && Get(i, out thing);
		}

		public static bool Get(int index, out IThing thing) {
			bool success = index > 0 && --index < things.Count;
			thing = success ? things[index] : default;
			return success;
		}

		public static string HeadingOfAdd(IThing thing) => HeadingOf(IndexOfAdd(thing));

		public static string HeadingOf(int index) => $"{index}. ";

		public static void Clear() => things.Clear();

		public static int IndexOfAdd(IThing thing) {
			int index = things.IndexOf(thing);
			if (index == -1) {
				index = things.Count;
				things.Add(thing);
			}
			return index + 1;
		}
	}
}
