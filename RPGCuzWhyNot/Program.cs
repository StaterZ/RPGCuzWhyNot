using System;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.MenuSystem;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Characters.Enemies;
using RPGCuzWhyNot.Things.Characters.NPCs;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Things.Item;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static void Main() {
			Terminal.IsCursorVisible = false; //default to not showing cursor


			//Load content
			Terminal.WriteLineDirect("{fg:Yellow}(Loading Content...)");
			if (!DataLoader.LoadGameData()) {
				Environment.Exit(1);
			}
			Terminal.WriteLineDirect("{fg:Green}(Done!)");
			ConsoleUtils.Sleep(100);
			Terminal.Clear();

			//construct player
			player = new Player(new Human(Humanoid.Gender.Male)) {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				stats = new Stats(10, 10, 10, 10)
			};


			//add start items to player
			player.Inventory.MoveItem(DataLoader.CreateItem("blue_potion"));
			player.Inventory.MoveItem(DataLoader.CreateItem("backpack"));
			player.Wielding.MoveItem((IWieldable)DataLoader.CreateItem("deluxeDebugDodad"));
			player.Wielding.MoveItem((IWieldable)DataLoader.CreateItem("greatsword"));


			//combat testing shortcut
			Fight fight = new Fight(player, new TheMother());
			fight.BeginCombat();


			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				player.Handle(commandText);
			}
		}
	}
}