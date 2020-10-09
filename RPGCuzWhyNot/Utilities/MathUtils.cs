using System;

namespace RPGCuzWhyNot.Utilities {
	public static class MathUtils {
		public static float Clamp(float value, float min, float max) {
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