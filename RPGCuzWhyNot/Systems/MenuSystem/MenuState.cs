using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.MenuSystem {
	public partial class Menu {
		public partial class MenuHandler {
			private class MenuState {
				public readonly Menu menu;

				private int _index;
				public int Index {
					get => _index;
					set {
						value = MathUtils.Mod(value, menu.items.Count);
						_index = value;
					}
				}

				public MenuItem Selected => menu.items[Index];

				public MenuState(Menu menu) {
					this.menu = menu;
				}

				public void Clear() {
					menu.Clear(Index);
				}

				public void ClearDescription() {
					menu.ClearDescription(Index);
				}

				public void Draw(IEnumerable<Menu> path) {
					menu.Draw(Index, path);
				}
			}
		}
	}
}