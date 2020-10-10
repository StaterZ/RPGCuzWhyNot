namespace RPGCuzWhyNot.Primitives {
	public struct Vec2 {
		public static Vec2 Zero => new Vec2(0, 0);
		public static Vec2 One => new Vec2(1, 1);
		public static Vec2 Right => new Vec2(1, 0);
		public static Vec2 Left => new Vec2(-1, 0);
		public static Vec2 Up => new Vec2(0, -1);
		public static Vec2 Down => new Vec2(0, 1);

		public int x, y;

		public Vec2(int x, int y) {
			this.x = x;
			this.y = y;
		}
		
		public override string ToString() {
			return $"({x},{y})";
		}

		public override bool Equals(object obj) {
			return obj is Vec2 vec2 && x.Equals(vec2.x) && y.Equals(vec2.y);
		}

		public override int GetHashCode() {
			int h1 = x.GetHashCode();
			int h2 = y.GetHashCode();
			return ((h1 << 5) + h1) ^ h2;
		}

		public static bool operator ==(Vec2 a, Vec2 b) {
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(Vec2 a, Vec2 b) {
			return a.x != b.x || a.y != b.y;
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

		public static Vec2 operator -(Vec2 a) {
			return new Vec2(-a.x, -a.y);
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