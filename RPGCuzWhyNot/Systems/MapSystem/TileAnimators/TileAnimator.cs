using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.MapSystem.TileAnimators {
	public abstract class TileAnimator {
		public abstract char Animate(char[] symbols, Vec2 pos, int frame);
	}
}
