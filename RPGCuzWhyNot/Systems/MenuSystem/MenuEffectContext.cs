using System.Collections.Generic;
using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.MenuSystem {
	public class MenuEffectContext {
		private readonly Vec2 drawPos;
		private readonly Stack<Menu> stack;

		public MenuEffectContext(Vec2 drawPos, Stack<Menu> stack) {
			this.drawPos = drawPos;
			this.stack = stack;
		}

		public void EnterMenu(Menu menu) {
			//clear last menu
			if (stack.Count > 0) {
				Terminal.CursorPosition = drawPos;
				stack.Peek().ClearDraw();
			}

			//draw new menu
			stack.Push(menu);
			Terminal.CursorPosition = drawPos;
			menu.Enter(stack);
		}

		public void ExitMenu() {
			if (stack.Count > 0) {
				Terminal.CursorPosition = drawPos;
				stack.Peek().ClearDraw();
			}
			Terminal.CursorPosition = drawPos;

			stack.Pop();
		}

		public void ExitEntireMenuStack() {
			while (stack.Count > 0) {
				ExitMenu();
			}
		}
	}
}
