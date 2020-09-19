using RPGCuzWhyNot.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGCuzWhyNot {
	public class Menu {
		private const string shortHands = "123456789abcdefghijklmopqrstuvwxyz";
		public readonly List<MenuItem> items;

		public Menu() {
			items = new List<MenuItem>();
		}

		public Menu(params MenuItem[] items) {
			this.items = items.ToList();
		}

		public void Draw(int? selectedIndex) {
			const string unselectedBegin = "  ";
			const string unselectedEnd = "  ";
			const string selectedBegin = "> ";
			const string selectedEnd = " <";
			const string shortHandPattern = " (X)";

			int longestItemLength = items.Aggregate(0, (acc, item) => {
				int length = item.name.PrintLength();
				return length > acc ? length : acc;
			});
			int longestBeginLength = Math.Max(unselectedBegin.Length, selectedBegin.Length);
			int longestEndLength = Math.Max(unselectedEnd.Length, selectedEnd.Length);
			int longestLine = longestBeginLength + longestItemLength + shortHandPattern.Length + longestEndLength;

			Terminal.WriteLineDirect(new string('#', longestLine));
			for (int i = 0; i < items.Count; i++) {
				bool isSelected = i == selectedIndex; //if selectedIndex is null isSelected will go false, hiding the arrows
				char shortHand = i >= 0 && i < shortHands.Length ? shortHands[i] : '!';

				if (isSelected) {
					Terminal.WriteDirect(selectedBegin);
				} else {
					Terminal.WriteDirect(unselectedBegin);
				}

				Terminal.WriteDirect(items[i].name.PadRight(longestItemLength));

				Terminal.WriteDirect(shortHandPattern.Replace('X', shortHand));

				if (isSelected) {
					Terminal.WriteLineDirect(selectedEnd);
				} else {
					Terminal.WriteLineDirect(unselectedEnd);
				}
			}
			Terminal.WriteLineDirect(new string('#', longestLine));
		}
	}
}
