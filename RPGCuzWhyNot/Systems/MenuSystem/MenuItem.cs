namespace RPGCuzWhyNot.Systems.MenuSystem {
	public class MenuItem {
		public readonly string name;
		public readonly string description;
		public readonly MenuItemEffect effect;

		public delegate void MenuItemEffect(Menu.MenuHandler handler);

		public MenuItem(string name, string description, MenuItemEffect effect) {
			this.name = name;
			this.description = description;
			this.effect = effect;
		}
	}
}