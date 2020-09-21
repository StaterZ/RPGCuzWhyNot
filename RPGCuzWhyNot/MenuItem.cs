namespace RPGCuzWhyNot {
	public class MenuItem {
		public readonly string name;
		public readonly string hoverDescription;
		public readonly MenuItemEffect effect;

		public delegate void MenuItemEffect(MenuEffectContext ctx);

		public MenuItem(string name, string hoverDescription, MenuItemEffect effect) {
			this.name = name;
			this.hoverDescription = hoverDescription;
			this.effect = effect;
		}
	}
}