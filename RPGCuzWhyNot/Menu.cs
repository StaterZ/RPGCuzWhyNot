using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGCuzWhyNot {
	public class Menu {
		private const string shortHands = "123456789abcdefghijklmopqrstuvwxyz";
		const string unselectedBegin = "  ";
		const string unselectedEnd = "  ";
		const string selectedBegin = "> ";
		const string selectedEnd = " <";
		const string shortHandPattern = "?. ";
		public readonly List<MenuItem> items;

		private int LongestItemLength {
			get {
				return items.Aggregate(0, (acc, item) => {
					int length = Terminal.GetFormattedLength(item.name);
					return length > acc ? length : acc;
				});
			}
		}

		public int Width {
			get {
				int longestBeginLength = Math.Max(unselectedBegin.Length, selectedBegin.Length);
				int longestEndLength = Math.Max(unselectedEnd.Length, selectedEnd.Length);
				int longestLine = longestBeginLength + LongestItemLength + longestEndLength + shortHandPattern.Length;

				return longestLine;
			}
		}

		public int Height {
			get {
				return items.Count + 2; //+2 for the top and bottom borders
			}
		}

		public Menu() {
			items = new List<MenuItem>();
		}

		public Menu(params MenuItem[] items) {
			this.items = items.ToList();
		}

		public void Draw(int? selectedIndex) {
			Terminal.WriteLineDirect(new string('#', Width));
			for (int i = 0; i < items.Count; i++) {
				bool isSelected = i == selectedIndex; //if selectedIndex is null isSelected will go false, hiding the arrows
				char shortHand = i >= 0 && i < shortHands.Length ? shortHands[i] : '!';

				if (isSelected) {
					Terminal.WriteDirect(selectedBegin);
				} else {
					Terminal.WriteDirect(unselectedBegin);
				}

				Terminal.WriteDirect(shortHandPattern.Replace('?', shortHand));

				Terminal.WriteDirect(items[i].name);

				Terminal.WriteDirect(new string(' ', LongestItemLength - Terminal.GetFormattedLength(items[i].name)));

				if (isSelected) {
					Terminal.WriteLineDirect(selectedEnd);
				} else {
					Terminal.WriteLineDirect(unselectedEnd);
				}
			}
			Terminal.WriteLineDirect(new string('#', Width));
		}

		public void ClearDraw() {
			for (int i = 0; i < Height; i++) {
				Terminal.ClearLine();
			}
		}

		public void Enter() {
			Stack<Menu> stack = new Stack<Menu>();
			stack.Push(this);

			Enter(stack);
		}

		public void Enter(Stack<Menu> stack) {
			Vec2 drawPos = Terminal.CursorPosition;
			bool isCursorVisible = false;
			int arrowIndex = 0;
			MenuEffectContext ctx = new MenuEffectContext(drawPos, stack);

			void WrapArrowIndex() {
				arrowIndex = ExtraMath.Mod(arrowIndex, items.Count);
			}

			void ExecuteMenuItem(int index) {
				Terminal.Beep(200, 50);
				items[index].effect(ctx);
			}

			using (new CursorVisibilityScope(false)) {
				while (stack.Count > 0 && stack.Peek() == this) {
					//draw
					Terminal.CursorPosition = drawPos;
					Draw(isCursorVisible ? arrowIndex : (int?)null);

					//get input
					ConsoleKeyInfo keyPress = Console.ReadKey(true);

					//try finding and useing a short hand
					int shortHandIndex = shortHands.IndexOf(keyPress.KeyChar);
					if (shortHandIndex >= 0 && shortHandIndex < items.Count) {
						ExecuteMenuItem(shortHandIndex);
					} else {
						//else, update the arrow key system
						switch (keyPress.Key) {
							//go up
							case ConsoleKey.UpArrow:
								Terminal.Beep(100, 50);
								isCursorVisible = true;
								arrowIndex--;
								WrapArrowIndex();
								break;

							//go down
							case ConsoleKey.DownArrow:
								Terminal.Beep(100, 50);
								isCursorVisible = true;
								arrowIndex++;
								WrapArrowIndex();
								break;

							//go back
							case ConsoleKey.LeftArrow:
								if (stack.Count > 1) {
									Terminal.Beep(200, 50);
									ctx.ExitMenu();
								} else {
									Terminal.Beep(50, 100);
								}
								break;

							//execute selected menu item
							case ConsoleKey.RightArrow:
							case ConsoleKey.Enter:
								isCursorVisible = true;
								ExecuteMenuItem(arrowIndex);
								break;
						}
					}
				}
			}
		}
	}
}
