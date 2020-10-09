using System;
using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems.MenuSystem {
	public partial class Menu {
		public partial class MenuHandler {
			private readonly Stack<MenuState> stack = new Stack<MenuState>();
			private Vec2 drawPos;
			private bool needsRedraw;

			public void Run() {
				drawPos = Terminal.CursorPosition;
				needsRedraw = true;

				while (stack.Count > 0) {
					MenuState menuState = stack.Peek();

					//draw
					if (needsRedraw) {
						Terminal.CursorPosition = drawPos;
						menuState.Draw(stack.Select(instance => instance.menu).Reverse());
						needsRedraw = false;
					}

					//get input
					ConsoleKeyInfo keyPress = Console.ReadKey(true);

					//try finding and using a short hand
					int shortHandIndex = shortHands.IndexOf(keyPress.KeyChar);
					if (shortHandIndex != -1 && shortHandIndex < menuState.menu.items.Count) {
						Terminal.Beep(200, 50);
						menuState.Index = shortHandIndex;
						menuState.Selected.effect(this);
					} else {
						//else, update the arrow key system
						switch (keyPress.Key) {
							//go up
							case ConsoleKey.UpArrow:
								Terminal.Beep(100, 50);
								menuState.Index--;
								Terminal.CursorPosition = drawPos;
								menuState.ClearDescription();
								needsRedraw = true;
								break;

							//go down
							case ConsoleKey.DownArrow:
								Terminal.Beep(100, 50);
								menuState.Index++;
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
								if (menuState.Index < menuState.menu.items.Count) {
									menuState.Selected.effect(this);
								}
								break;
						}
					}
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
		}
	}
}