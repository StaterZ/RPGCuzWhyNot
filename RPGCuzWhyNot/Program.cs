using System;
using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.NPCs;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data;
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
			//Load content
			Terminal.WriteLineDirect("{fg:Yellow}(Loading Content...)");
			if (!DataLoader.LoadGameData()) {
				Environment.Exit(1);
			}
			Terminal.WriteLineDirect("{fg:Green}(Done!)");
			ConsoleUtils.Sleep(100);
			Terminal.Clear();

			//add npcs to smithy
			Location smithy = DataLoader.GetLocation("village_smithy");
			smithy.AddNPC(new Orchibald(), "A smith can be seen by a large forge", "You walk up to the smith. He turns around to look at you.");
			smithy.AddNPC(new SmithyCustomer(), "A customer casually stands leaning against a pillar.", "You walk up to the customer. She glares angrily as you approach...");

			//construct player
			player = new Player(new Human(Humanoid.Gender.Male)) {
				Name = "Bengt",
				location = DataLoader.GetLocation("village"),
				stats = new Stats(10, 10, 10, 10)
			};

			//add start items to player
			player.Inventory.MoveItem(DataLoader.CreateItem("blue_potion"));
			player.Inventory.MoveItem(DataLoader.CreateItem("backpack"));


			TestMenu();


			//combat testing shortcut
			//player.Wielding.MoveItem((IWieldable)DataLoader.CreateItem("greatsword"));
			//Fight fight = new Fight(player, new TheMother());
			//fight.BeginCombat();

			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = ConsoleUtils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				player.Handle(commandText);
			}
		}

		private static void TestMenu() {
			Menu greatswordMenu = new Menu("Greatsword",
				new MenuItem("{fg:DarkYellow}(Light Attack)", ctx => {
					//do light attack

					ctx.ExitEntireMenuStack();
				}),
				new MenuItem("{fg:Red}(Heavy Attack)", ctx => {
					//do heavy attack

					ctx.ExitEntireMenuStack();
				}),
				new MenuItem("{fg:White;bg:Red}(Throw Sword)", ctx => {
					//throw sword

					ctx.ExitEntireMenuStack();
				})
			);
			Menu staffMenu = new Menu("Staff",
				new MenuItem("{fg:Cyan}(Channel)", ctx => {
					//do channel

					ctx.ExitEntireMenuStack();
				}),
				new MenuItem("{fg:Green}(Heal Ward)", ctx => {
					//do heal ward

					ctx.ExitEntireMenuStack();
				}),
				new MenuItem("{fg:DarkYellow}(Fireball)", ctx => {
					//do fireball

					ctx.ExitEntireMenuStack();
				}),
				new MenuItem("{fg:Black;bg:DarkGray}(Fine-Point Void)", ctx => {
					//do void thingy

					ctx.ExitEntireMenuStack();
				})
			);
			Menu attackMenu = new Menu("Attack",
				new MenuItem("{fg:DarkGray}(Greatsword)", ctx => ctx.EnterMenu(greatswordMenu)),
				new MenuItem("{fg:Yellow}(Staff)", ctx => ctx.EnterMenu(staffMenu))
			);
			Menu rootMenu = new Menu("Root",
				new MenuItem("{fg:Red}(Attack)", ctx => ctx.EnterMenu(attackMenu)),
				new MenuItem("{fg:Green}(Items)", null),
				new MenuItem("{fg:Blue}(Potions)", null),
				new MenuItem("{fg:Yellow}(Armor)", null)
			);

			rootMenu.Enter();
		}
	}
}