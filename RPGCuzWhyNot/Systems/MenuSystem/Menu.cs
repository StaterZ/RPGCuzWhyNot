using System;
using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.MenuSystem {
	public class Menu {
		private const string unselectedBegin = "  ";
		private const string unselectedEnd = "  ";
		private const string selectedBegin = "> ";
		private const string selectedEnd = " <";
		private const string shortHandPattern = "?. ";
		private const string pathBegin = "{fg:Blue}([)";
		private const string pathSeparator = "{fg:Blue}(->)";
		private const string pathEnd = "{fg:Blue}(])";
		private const string emptyMenuMessage = "{fg:DarkGray}(-- Empty --)";

		public readonly string name;
		public readonly List<MenuItem> items;

		private int ItemCount => Math.Max(items.Count, 1);

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

		public int BaseHeight => ItemCount + 2 + 1; //+2 for the top and bottom borders, +1 for the path display

		public Menu(string name, params MenuItem[] items) {
			this.name = name;
			this.items = items.ToList();
		}

		public void Draw(int selectedIndex, IEnumerable<Menu> path) {
			Terminal.WriteLineWithoutDelay(Utils.StringifyArray(pathBegin, pathSeparator, pathEnd, path.Select(menu => menu.name).ToArray()));
			Terminal.WriteLineWithoutDelay(new string('#', Width));
			if (items.Count > 0) {
				for (int i = 0; i < ItemCount; i++) {
					bool isSelected = i == selectedIndex;
					char shortHand = i < MenuHandler.shortHands.Length ? MenuHandler.shortHands[i] : '!';

					Terminal.WriteWithoutDelay(isSelected ? selectedBegin : unselectedBegin);
					Terminal.WriteWithoutDelay(shortHandPattern.Replace('?', shortHand));
					Terminal.WriteWithoutDelay(items[i].name);
					Terminal.WriteWithoutDelay(new string(' ', LongestItemLength - Terminal.GetFormattedLength(items[i].name)));
					Terminal.WriteLineWithoutDelay(isSelected ? selectedEnd : unselectedEnd);
				}
			} else {
				Terminal.WriteLineWithoutDelay(emptyMenuMessage);
			}
			Terminal.WriteLineWithoutDelay(new string('#', Width));

			if (selectedIndex < items.Count) {
				Terminal.WriteLine(items[selectedIndex].description);
			}
		}

		public void Clear(int selectedIndex) {
			for (int i = 0; i < BaseHeight; i++) {
				Terminal.ClearLine();
			}

			ClearDescription(selectedIndex, false);
		}

		public void ClearDescription(int selectedIndex, bool offsetToLocation = true) {
			if (offsetToLocation) {
				Terminal.CursorPosition += Vec2.Down * BaseHeight;
			}

			int numOfLines = selectedIndex != -1 && selectedIndex < items.Count
				? items[selectedIndex].description.Count(c => c == '\n') + 1
				: 0;

			for (int i = 0; i < numOfLines; i++) {
				Terminal.ClearLine();
			}
		}

		public void EnterAsRoot() {
			MenuHandler handler = new MenuHandler();
			handler.EnterMenu(this);
			handler.RunUntilExitAsRoot();
		}
	}
}
