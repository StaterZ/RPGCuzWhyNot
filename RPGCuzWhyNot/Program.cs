using System;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.MapSystem;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Characters.Enemies;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static void Main() {
			//Load content
			Terminal.WriteLineWithoutDelay("{fg:Yellow}(Loading Content...)");
			if (!DataLoader.LoadGameData()) {
				Environment.Exit(1);
			}
			Terminal.WriteLineWithoutDelay("{fg:Green}(Done!)");
			ConsoleUtils.Sleep(100);
			Terminal.Clear();

			//construct player
			player = new Player {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				race = new Human {
					gender = Humanoid.Gender.Male
				}
			};

			//add start items to player
			player.Inventory.MoveItem(DataLoader.CreateItem("blue_potion"));
			player.Inventory.MoveItem(DataLoader.CreateItem("backpack"));

			//Map testing shortcut
			MapTest();

			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				player.Handle(commandText);
			}
		}

		private static void MapTest() {
			//Map map = new Map(
			//	"ddddddddddwwwwwwwwww",
			//	"ddd@ccccccwwwwwwwwww",
			//	"dddgcwwwccwwwwwwwwww",
			//	"ggggcggwwcwwwwwwwwww",
			//	"ggggggggggwwwwwwwwww",
			//	"gdgggggg  wwwwwwwwww",
			//	"gggggs    wwwwwwwwww",
			//	"dgggsss   wwwwwwwwww",
			//	"dgggssss  wwwwwwwwww",
			//	"ddgggsss  wwwwwwwwww"
			//);

			Map map = new Map(
				"ggwwswwgggggwwswwgggdggggggg  ",
				"gggscsgggggggscsggggdgggggggg ",
				"ggggsgggggggggsgggggdggwwwgggg",
				"ggggdgggggggggdgggggdgwwwwwggg",
				"dddddddddddddddddddddgwwcwwggg",
				"ggggdggggg@gggdgggggdgwwwwwggg",
				"ggggsgggggggggsgggggdggwwwgggg",
				"gggscsgggggggscsggggdgggggggg ",
				"ggwwswwgggggwwswwgggdggggggg  ",
				"ggwwswwgggggwwswwgggdggggggg  ",
				"gggscsgggggggscsggggdgggggggg ",
				"ggggsgggggggggsgggggdggwwwgggg",
				"ggggdgggggggggdgggggdgwwwwwggg",
				"dddddddddddddddddddddgwwcwwggg",
				"ggggdggggg@gggdgggggdgwwwwwggg",
				"oooooooooooooooooooooooooooooo",
				"oooooooooooooooooooooooooooooo",
				"oooooooooooooooooooooooooooooo",
				"oooooooooooooooooooooooooooooo",
				"oooooooooooooooooooooooooooooo",
				"oooooooooooooooooooooooooooooo",
				"ooooooooooxxxxxxxxxooooooooooo",
				"oooooooooooooooooooooooooooooo",
				"oooooooooooooooooooooooooooooo",
				"oooooooooooooooooooooooooooooo"
			);

			map.Render();
		}
	}
}