using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCuzWhyNot.Utilities {
	public static class Utils {
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
	}
}