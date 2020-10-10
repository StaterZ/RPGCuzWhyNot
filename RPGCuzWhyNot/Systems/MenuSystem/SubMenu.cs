namespace RPGCuzWhyNot.Systems.MenuSystem {
	public class SubMenu : MenuItem {
		public SubMenu(Menu subMenu, string description) : base(subMenu.name, description, handler => handler.EnterMenu(subMenu)) {
		}
	}
}