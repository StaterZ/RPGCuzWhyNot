using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCuzWhyNot.Primitives {
	public struct Color {
		public byte r, g, b;

		public Color(byte r, byte g, byte b) {
			this.r = r;
			this.g = g;
			this.b = b;
		}

		public Color(byte value) {
			r = value;
			g = value;
			b = value;
		}

		public Color(int rgb) {
			r = (byte)(rgb >> 16);
			g = (byte)(rgb >> 8);
			b = (byte)(rgb >> 0);
		}

		public Color(ConsoleColor consoleColor) {
			Color color = consoleColors[(int)consoleColor];
			r = color.r;
			g = color.g;
			b = color.b;
		}

		public Color(string colorName) {
			Color color = GetPredefinedColor(colorName);
			r = color.r;
			g = color.g;
			b = color.b;
		}

		public bool Equals(Color other) {
			return r == other.r && g == other.g && b == other.b;
		}

		public override bool Equals(object obj) {
			return obj is Color other && Equals(other);
		}

		public override int GetHashCode() {
			return HashCode.Combine(r, g, b);
		}

		public static bool operator ==(Color left, Color right) {
			return left.Equals(right);
		}

		public static bool operator !=(Color left, Color right) {
			return !left.Equals(right);
		}


		#region Predefined colors

		// Console colors
		public static readonly Color Black = new Color(0x0C0C0C); // default: 0x0C0C0C
		public static readonly Color DarkBlue = new Color(0x0037DA); // default: 0x0037DA
		public static readonly Color DarkGreen = new Color(0x13A10E); // default: 0x13A10E
		public static readonly Color DarkCyan = new Color(0x3A96DD); // default: 0x3A96DD
		public static readonly Color DarkRed = new Color(0xC50F1F); // default: 0xC50F1F
		public static readonly Color DarkMagenta = new Color(0x881798); // default: 0x881798
		public static readonly Color DarkYellow = new Color(0xC19C00); // default: 0xC19C00
		public static readonly Color Gray = new Color(0xCCCCCC); // default: 0xCCCCCC
		public static readonly Color DarkGray = new Color(0x767676); // default: 0x767676
		public static readonly Color Blue = new Color(0x3B78FF); // default: 0x3B78FF
		public static readonly Color Green = new Color(0x16C60C); // default: 0x16C60C
		public static readonly Color Cyan = new Color(0x61D6D6); // default: 0x61D6D6
		public static readonly Color Red = new Color(0xE74856); // default: 0xE74856
		public static readonly Color Magenta = new Color(0xD800BF); // default: 0xB4009E
		public static readonly Color Yellow = new Color(0xF9F140); // default: 0xF9F1A5
		public static readonly Color White = new Color(0xF2F2F2); // default: 0xF2F2F2

		// Additional colors
		public static readonly Color Orange = new Color(0xED820E);

		#endregion

		private static readonly Color[] consoleColors = {
			Black,
			DarkBlue,
			DarkGreen,
			DarkCyan,
			DarkRed,
			DarkMagenta,
			DarkYellow,
			Gray,
			DarkGray,
			Blue,
			Green,
			Cyan,
			Red,
			Magenta,
			Yellow,
			White
		};

		private static readonly Dictionary<string, Color> colors;

		static Color() {
			colors = typeof(Color)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(field => field.IsInitOnly && field.FieldType == typeof(Color))
				.ToDictionary(field => field.Name.ToLower(),
				              field => (Color)field.GetValue(null)!);
		}

		public static Color GetPredefinedColor(string name) {
			if (colors.TryGetValue(name.ToLower(), out Color color))
				return color;
			throw new ArgumentException("Color not found.");
		}

		public static bool TryGetPredefinedColor(string name, out Color color) {
			return colors.TryGetValue(name.ToLower(), out color);
		}
	}
}