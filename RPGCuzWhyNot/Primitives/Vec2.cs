namespace RPGCuzWhyNot.Primitives {
	public struct Vec2 {
		public static Vec2 Zero => new Vec2(0, 0);
		public static Vec2 One => new Vec2(1, 1);
		public static Vec2 Right => new Vec2(1, 0);
		public static Vec2 Left => new Vec2(-1, 0);
		public static Vec2 Up => new Vec2(0, 1);
		public static Vec2 Down => new Vec2(0, -1);

		public int x, y;

		public Vec2(int x, int y) {
			this.x = x;
			this.y = y;
		}

		public static Vec2 operator +(Vec2 a, Vec2 b) {
			return new Vec2(a.x + b.x, a.y + b.y);
		}

		public static Vec2 operator +(Vec2 a, int b) {
			return new Vec2(a.x + b, a.y + b);
		}

		public static Vec2 operator -(Vec2 a, Vec2 b) {
			return new Vec2(a.x - b.x, a.y - b.y);
		}

		public static Vec2 operator -(Vec2 a, int b) {
			return new Vec2(a.x - b, a.y - b);
		}

		public static Vec2 operator *(Vec2 a, Vec2 b) {
			return new Vec2(a.x * b.x, a.y * b.y);
		}

		public static Vec2 operator *(Vec2 a, int b) {
			return new Vec2(a.x * b, a.y * b);
		}

		public static Vec2 operator /(Vec2 a, Vec2 b) {
			return new Vec2(a.x / b.x, a.y / b.y);
		}

		public static Vec2 operator /(Vec2 a, int b) {
			return new Vec2(a.x / b, a.y / b);
		}
	}
}