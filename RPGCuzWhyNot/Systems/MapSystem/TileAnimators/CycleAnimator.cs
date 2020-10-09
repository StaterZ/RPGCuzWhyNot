using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.MapSystem.TileAnimators {
	public class CycleAnimator : TileAnimator {
		public override char Animate(char[] symbols, Vec2 pos, int frame) {
			return symbols[frame % symbols.Length];
		}
	}
}