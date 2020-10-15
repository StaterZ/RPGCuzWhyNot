using System;
using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.MenuSystem {
	public class MenuHandler {
		public const string shortHands = "123456789abcdefghijklmopqrstuvwxyz";
		private readonly Stack<MenuState> stack = new Stack<MenuState>();
		private Vec2 drawPos;
		private bool needsRedraw;

		public void RunUntilExitAsRoot() {
			drawPos = Terminal.CursorPosition;
			needsRedraw = true;
			RunUntilExit();
		}

		public void RunUntilExit() {
			int startStackCount = stack.Count;
			while (stack.Count >= startStackCount) {
				MenuState menuState = stack.Peek();

				//update
				DrawIfNeeded(menuState);

				//navigate
				ConsoleKeyInfo keyPress = Terminal.ReadKey(true);
				if (!TryRunShortHand(menuState, keyPress)) {
					UpdateArrowNavigation(menuState, keyPress);
				}
			}

			Terminal.CursorPosition = drawPos;
		}

		private bool TryRunShortHand(MenuState menuState, ConsoleKeyInfo keyPress) {
			int shortHandIndex = shortHands.IndexOf(keyPress.KeyChar);
			if (shortHandIndex == -1) return false;
			if (shortHandIndex >= menuState.menu.items.Count) return false;

			Terminal.Beep(200, 50);
			menuState.SelectedIndex = shortHandIndex;
			menuState.Selected.effect(this);

			return true;
		}

		private void DrawIfNeeded(MenuState menuState) {
			if (!needsRedraw) return;

			Terminal.CursorPosition = drawPos;
			menuState.Draw(stack.Select(instance => instance.menu).Reverse());
			needsRedraw = false;
		}

		private void UpdateArrowNavigation(MenuState menuState, ConsoleKeyInfo keyPress) {
			switch (keyPress.Key) {
				//go up
				case ConsoleKey.UpArrow:
					Terminal.Beep(100, 50);
					menuState.SelectedIndex--;
					Terminal.CursorPosition = drawPos;
					menuState.ClearDescription();
					needsRedraw = true;
					break;

				//go down
				case ConsoleKey.DownArrow:
					Terminal.Beep(100, 50);
					menuState.SelectedIndex++;
					Terminal.CursorPosition = drawPos;
					menuState.ClearDescription();
					needsRedraw = true;
					break;

				//go back
				case ConsoleKey.LeftArrow:
				case ConsoleKey.Escape:
				case ConsoleKey.Backspace:
					if (stack.Count > 1) {
						Terminal.Beep(200, 50);
						ExitMenu();
					} else {
						Terminal.Beep(50, 100);
					}
					break;

				//execute selected menu item
				case ConsoleKey.RightArrow:
				case ConsoleKey.Enter:
					Terminal.Beep(200, 50);
					if (menuState.SelectedIndex < menuState.menu.items.Count) {
						menuState.Selected.effect(this);
					}
					break;
			}
		}

		public void EnterMenu(Menu menu) {
			//clear last menu
			if (stack.Count > 0) {
				Terminal.CursorPosition = drawPos;
				stack.Peek().Clear();
				needsRedraw = true;
			}

			//draw new menu
			stack.Push(new MenuState(menu));
		}

		public void ExitMenu() {
			Terminal.CursorPosition = drawPos;
			stack.Pop().Clear();
			needsRedraw = true;
		}

		public void ExitEntireMenuStack() {
			while (stack.Count > 0) {
				ExitMenu();
			}
		}

		private class MenuState {
			public readonly Menu menu;

			private int selectedIndex;
			public int SelectedIndex {
				get => selectedIndex;
				set {
					value = Utils.Mod(value, menu.items.Count);
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