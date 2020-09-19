using System;

namespace RPGCuzWhyNot.Utilities {
	public static class ExtraMath {
		public static float Clamp(float value, float min, float max) {
			return Math.Min(Math.Max(value, min), max);
		}

		public static int Clamp(int value, int min, int max) {
			return Math.Min(Math.Max(value, min), max);
		}

		public static int Mod(int a, int b) {
			return a - b * (int)Math.Floor((float)a / b);
		}
	}
}