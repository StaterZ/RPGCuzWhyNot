using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.MapSystem.TileAnimators {
	public class TextScrollAnimator : TileAnimator {
		public override char Animate(char[] symbols, Vec2 pos, int frame) {
			return symbols[(pos.x + frame) % symbols.Length];
		}
	}
}