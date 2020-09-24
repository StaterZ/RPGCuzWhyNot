using System;
using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.NPCs;
using RPGCuzWhyNot.Systems;
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

			while (true) {
				TestMenu();
			}

			//combat testing shortcut
			player.Wielding.MoveItem((IWieldable)DataLoader.CreateItem("greatsword"));
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

		private static void TestMenu() {
			Menu greatswordMenu = new Menu("Greatsword",
				new MenuItem("{fg:DarkYellow}(Light Attack)", "description of action with stats, notes and other info", ctx => {
					ctx.ExitEntireMenuStack();

					Terminal.WriteLine("do light attack here later");
					ConsoleUtils.Sleep(1000);
				}),
				new MenuItem("{fg:Red}(Heavy Attack)", "description of action with stats, notes and other info", ctx => {
					ctx.ExitEntireMenuStack();

					Terminal.WriteLine("do heavy attack here later");
					ConsoleUtils.Sleep(1000);
				}),
				new MenuItem("{fg:White;bg:Red}(Throw Sword)", "description of action with stats, notes and other info", ctx => {
					ctx.ExitEntireMenuStack();

					Terminal.WriteLine("do throw sword here later");
					ConsoleUtils.Sleep(1000);
				})
			);

			Menu staffMenu = new Menu("Staff",
				new MenuItem("{fg:Cyan}(Channel)", "description of action with stats, notes and other info", ctx => {
					ctx.ExitEntireMenuStack();

					Terminal.WriteLine("do channel here later");
					ConsoleUtils.Sleep(1000);
				}),
				new MenuItem("{fg:Green}(Heal Ward)", "description of action with stats, notes and other info", ctx => {
					ctx.ExitEntireMenuStack();

					Terminal.WriteLine("do heal ward here later");
					ConsoleUtils.Sleep(1000);
				}),
				new MenuItem("{fg:DarkYellow}(Fireball)", "description of action with stats, notes and other info", ctx => {
					ctx.ExitEntireMenuStack();

					Terminal.WriteLine("do fireball here later");
					ConsoleUtils.Sleep(1000);
				}),
				new MenuItem("{fg:Black;bg:DarkGray}(Fine-Point Void)", "description of action with stats, notes and other info", ctx => {
					ctx.ExitEntireMenuStack();

					Terminal.WriteLine("do void thingy here later");
					ConsoleUtils.Sleep(1000);
				})
			);

			Menu attackMenu = new Menu("Attack",
				new MenuItem("{fg:DarkGray}(Greatsword)", "Do something with the Greatsword", ctx => ctx.EnterMenu(greatswordMenu)),
				new MenuItem("{fg:Yellow}(Staff)", "Do something with the Staff", ctx => ctx.EnterMenu(staffMenu))
			);

			Menu itemMenu = new Menu("Items");

			Menu potionMenu = new Menu("Potions");

			Menu armorMenu = new Menu("Armor");

			Menu rootMenu = new Menu("Root",
				new MenuItem("{fg:Red}(Attack)", "Attack Something", ctx => ctx.EnterMenu(attackMenu)),
				new MenuItem("{fg:Green}(Items)", "Use an Item", ctx => ctx.EnterMenu(itemMenu)),
				new MenuItem("{fg:Blue}(Potions)", "Consume/Use a Potion", ctx => ctx.EnterMenu(potionMenu)),
				new MenuItem("{fg:Yellow}(Armor)", "Activate an armor ability", ctx => ctx.EnterMenu(armorMenu))
			);

			rootMenu.Enter();
		}
	}
}