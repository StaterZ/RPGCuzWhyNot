namespace RPGCuzWhyNot {
	public struct Fraction {
		public readonly int numerator;
		public readonly int denominator;

		public Fraction(int numerator, int denominator) {
			this.numerator = numerator;
			this.denominator = denominator;
		}

		public static int operator *(Fraction a, int b) => (b * a.numerator + a.denominator - 1) / a.denominator;
	}
}
