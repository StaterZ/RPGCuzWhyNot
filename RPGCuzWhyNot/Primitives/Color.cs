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
	}
}