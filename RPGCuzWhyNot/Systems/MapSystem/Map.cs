using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems.MapSystem.TileAnimators;
using RPGCuzWhyNot.Utilities;
using System;
using System.Collections.Generic;

namespace RPGCuzWhyNot.Systems.MapSystem {
	public class Map {
		private const int tickDelay = 250;
		private static readonly TileMaterial errorTile = new TileMaterial("Error ", ConsoleColor.Red, new TextScrollAnimator());
		private static readonly Dictionary<char, TileMaterial> materialLookup = new Dictionary<char, TileMaterial>() {
			{ 'd', new TileMaterial(".", ConsoleColor.DarkYellow) },
			{ 'g', new TileMaterial("¸.,", ConsoleColor.Green, new SweepAnimator(5, 2)) },
			{ 's', new TileMaterial(".", ConsoleColor.Yellow) },
			{ 'w', new TileMaterial("~≈", ConsoleColor.Cyan, new BubbleAnimator()) },
			{ 'o', new TileMaterial("~≈", ConsoleColor.Cyan, new SweepAnimator(5, 2)) },
			{ ' ', new TileMaterial(" ", ConsoleColor.Black) },
			{ 'c', new TileMaterial("#", ConsoleColor.Gray) },
			{ '@', new TileMaterial("@", ConsoleColor.Magenta) }
		};

		private readonly string[] data;
		public Map(params string[] data) {
			this.data = data;
		}

		public void Render() {
			bool isRendering = true;

			Console.OutputEncoding = System.Text.Encoding.UTF8; //display unicode characters properly
			Vec2 renderPos = Terminal.CursorPosition;
			int frame = 0;
			Terminal.PushState();
			while (isRendering) {
				Terminal.CursorPosition = renderPos;
				for (int y = 0; y < data.Length; y++) {
					for (int x = 0; x < data[y].Length; x++) {
						Vec2 pos = new Vec2(x, y);

						char tileId = data[y][x];
						if (!materialLookup.TryGetValue(tileId, out TileMaterial tileType)) {
							tileType = errorTile;
						}

						Terminal.ForegroundColor = tileType.color;
						Terminal.WriteWithoutDelay(tileType.GetSymbol(pos, frame));
						Terminal.WriteWithoutDelay(' ');
					}
					Terminal.WriteLineWithoutDelay();
				}
				frame++;
				ConsoleUtils.Sleep(tickDelay);
			}
			Terminal.PopState();
		}
	}
}