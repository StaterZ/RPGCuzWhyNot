using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Utilities;
using System;

namespace RPGCuzWhyNot.Systems.MapSystem.TileAnimators {
	public class SweepAnimator : TileAnimator {
		private readonly int lowFrames;
		private readonly int highFrames;

		public SweepAnimator(int lowFrames, int highFrames) {
			this.lowFrames = lowFrames;
			this.highFrames = highFrames;
		}

		public override char Animate(char[] symbols, Vec2 pos, int frame) {
			int lowFrames1 = lowFrames - 1;
			int highFrames1 = highFrames - 1;

			int l = symbols.Length - 1;
			int x = pos.x + pos.y;

			int a = MathUtils.Mod(x - frame, l * 2 + lowFrames1 + highFrames1);
			int b = Math.Max(a - lowFrames1, 0);
			float c = Math.Abs(b - (l + highFrames1 / 2f));
			int d = (int)Math.Max(c - (highFrames1 / 2f), 0);
			int e = l - d;

			return symbols[e];
		}
	}
}