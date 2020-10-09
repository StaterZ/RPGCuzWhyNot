using RPGCuzWhyNot.Primitives;
using System;

namespace RPGCuzWhyNot.Systems.MapSystem.TileAnimators {
	public class BubbleAnimator : TileAnimator {
		private static readonly Random rand = new Random();

		public override char Animate(char[] symbols, Vec2 pos, int frame) {
			return symbols[rand.Next(symbols.Length)];
		}
	}
}