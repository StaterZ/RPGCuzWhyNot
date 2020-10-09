using System.Collections.Generic;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.MenuSystem {
	public partial class MenuHandler {
		private class MenuState {
			public readonly Menu menu;

			private int selectedIndex;
			public int SelectedIndex {
				get => selectedIndex;
				set {
					value = MathUtils.Mod(value, menu.items.Count);
					selectedIndex = value;
				}
			}

			public MenuItem Selected => menu.items[SelectedIndex];

			public MenuState(Menu menu) {
				this.menu = menu;
			}

			public void Clear() {
				menu.Clear(SelectedIndex);
			}

			public void ClearDescription() {
				menu.ClearDescription(SelectedIndex);
			}

			public void Draw(IEnumerable<Menu> path) {
				menu.Draw(SelectedIndex, path);
			}
		}
	}
}