using System;

namespace RPGCuzWhyNot {
	public class MenuItem {
		public readonly string name;
		public readonly MenuItemEffect effect;

		public delegate void MenuItemEffect(MenuEffectContext ctx);

		public MenuItem(string name, MenuItemEffect effect) {
			this.name = name;
			this.effect = effect;
		}
	}
}