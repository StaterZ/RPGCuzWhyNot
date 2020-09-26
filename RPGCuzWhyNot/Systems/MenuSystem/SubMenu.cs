namespace RPGCuzWhyNot.Systems.MenuSystem {
	public class SubMenu : MenuItem {
		public SubMenu(Menu subMenu, string hoverDescription) : base(subMenu.name, hoverDescription, ctx => ctx.EnterMenu(subMenu)) {
		}
	}
}