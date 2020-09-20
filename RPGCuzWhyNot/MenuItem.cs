using System;

namespace RPGCuzWhyNot {
	public class MenuItem {
		public readonly string name;
		public readonly Action onSelect;

		public MenuItem(string name, Action onSelect) {
			this.name = name;
			this.onSelect = onSelect;
		}
	}
}