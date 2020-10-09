using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems.MapSystem.TileAnimators;
using System;
using System.Linq;

namespace RPGCuzWhyNot.Systems.MapSystem {
	public class TileMaterial {
		public readonly ConsoleColor color;
		public readonly char[] symbols;
		public readonly TileAnimator animatior;

		public TileMaterial(string symbols, ConsoleColor color, TileAnimator animatior = null) {
			this.symbols = symbols.ToArray();
			this.color = color;
			this.animatior = animatior;
		}

		public char GetSymbol(Vec2 pos, int frame) {
			return animatior?.Animate(symbols, pos, frame) ?? symbols[0];
		}
	}
}