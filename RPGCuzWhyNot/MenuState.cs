using System;

namespace RPGCuzWhyNot {
	public class MenuState {
		public readonly Menu menu;
		public int? index;

		public MenuState(Menu menu, int? index) {
			this.menu = menu;
			this.index = index;
		}

		public void Draw() {
			menu.Draw(index);
		}
	}
}