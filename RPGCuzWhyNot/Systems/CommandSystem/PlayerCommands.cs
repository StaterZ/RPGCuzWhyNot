using RPGCuzWhyNot.Systems.CommandSystem.Commands;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem {
	public class PlayerCommands {
		public readonly CommandHandler commandHandler = new CommandHandler();

		public static Player Player => Program.player;

		public void LoadCommands() {
			commandHandler.AddCommands(new Command[] {
				new GoCommand(),
				new WhereCommand(),
				new ListCommand(),
				new WearCommand(),
				new UnwearCommand(),
				new WearingCommand(),
				new WieldCommand(),
				new UnwieldCommand(),
				new WieldingCommand(),
				new EquipCommand(),
				new UnequipCommand(),
				new EquippedCommand(),
				new MoveCommand(),
				new TakeCommand(),
				new DropCommand(),
				new InventoryCommand(),
				new ItemsCommand(),
				new InspectCommand(),
				new HelpCommand(commandHandler),
				new ClearCommand(),
				new SpeakCommand(),
				new EverythingCommand(),
				new ThrowCommand(),
				new TypeCommand(),
				new QuitCommand(),
			});
		}

		public void Handle(string message) {
			if (!commandHandler.TryHandle(message)) {
				Terminal.WriteLine("I don't understand.");
			}
		}

		#region Helper methods

		public static string ItemSource(IItem item) {
			if (item.ContainedInventory == Player.Inventory)
				return " from your inventory";

			if (item.ContainedInventory == Player.location.items)
				return " from the ground";

			return "";
		}

		public static void Wear(IThing thing) {
			if (thing is IWearable wearable) {
				string source = ItemSource(wearable);
				if (Player.Wearing.MoveItem(wearable)) {
					int covers = (int)wearable.CoveredParts;
					string target = "";
					if ((wearable.CoveredParts & WearableSlots.Chest) == 0) {
						target = " your " + ((WearableSlots)(covers & ~(covers - 1))).ToString().ToLower();
					}

					Terminal.WriteLine($"You take {wearable.Name}{source} and put it on{target}.");
				}
			} else {
				Terminal.WriteLine($"{thing.Name} can not be worn.");
			}
		}

		public static void Wield(IThing thing) {
			if (thing is IWieldable wieldable) {
				string source = ItemSource(wieldable);
				if (Player.Wielding.MoveItem(wieldable)) {
					string handPlural = wieldable.HandsRequired != 1 ? "s" : "";
					Terminal.WriteLine($"You take {wieldable.Name}{source} and wield it in your hand{handPlural}.");
				}
			} else {
				Terminal.WriteLine($"{thing.Name} can not be wielded.");
			}
		}

		public static void ListInventory() {
			if (Player.Inventory.Count <= 0) {
				Terminal.WriteLine("Your inventory is empty.");
			} else {
				Terminal.WriteLine("Your inventory contains:");
				foreach (IItem item in Player.Inventory) {
					Terminal.WriteLine($"  {NumericCallNames.HeadingOfAdd(item)}{item.ListingName}");
				}

				foreach (IItem item in Player.Inventory) {
					if (item is IItemWithInventory inv) {
						ListItemWithInventory("", " in inventory", inv);
					}
				}
			}

			foreach (IItem item in Player.Wielding) {
				if (item is IItemWithInventory inv) {
					ListItemWithInventory("Wielded ", "", inv);
				}
			}

			foreach (IItem item in Player.Wearing) {
				if (item is IItemWithInventory inv) {
					ListItemWithInventory("Worn ", "", inv);
				}
			}
		}

		public static void ListItemWithInventory(string prefix, string suffix, IItemWithInventory inv) {
			Terminal.WriteLine();
			Terminal.Write($"{NumericCallNames.HeadingOfAdd(inv)}{prefix}{inv.ListingName}{suffix}");
			if (inv.Inventory.Count <= 0) {
				Terminal.WriteLine(" is empty.");
				return;
			}

			Terminal.WriteLine(" contains:");

			foreach (IItem item in inv) {
				Terminal.WriteLine($"  {NumericCallNames.HeadingOfAdd(item)}{item.ListingName}");
			}

			foreach (IItem item in inv) {
				if (item is IItemWithInventory nested) {
					ListItemWithInventory("", $" inside {prefix}{item.Name}{suffix}", nested);
				}
			}
		}

		public static void ListWearing() {
			if (Player.Wearing.Count == 0) {
				Terminal.WriteLine("You are not wearing anything");
				return;
			}

			Terminal.WriteLine("You are currently wearing:");
			foreach (IWearable wearable in Player.Wearing) {
				Terminal.WriteLine($"  {NumericCallNames.HeadingOfAdd(wearable)}{wearable.ListingWithStats}");
			}
		}

		public static void ListWielding() {
			if (Player.Wielding.Count == 0) {
				Terminal.WriteLine("You are unarmed.");
				return;
			}

			Terminal.WriteLine("You are currently wielding:");
			foreach (IWieldable wieldable in Player.Wielding) {
				Terminal.WriteLine($"  {NumericCallNames.HeadingOfAdd(wieldable)}{wieldable.ListingWithStats}");
			}
		}

		public static void ListLocationItems() {
			if (Player.location.items.Count <= 0) {
				Terminal.WriteLine("You look around but can't find anything of use.");
				return;
			}

			Terminal.WriteLine("You look around and see:");
			foreach (IItem item in Player.location.items) {
				Terminal.WriteLine($"  {NumericCallNames.HeadingOfAdd(item)}{item.ListingName}");
			}

			foreach (IItem item in Player.location.items) {
				if (item is IItemWithInventory inv) {
					ListItemWithInventory("", " on the ground", inv);
				}
			}
		}

		public static void ListLocations() {
			if (Player.location.Paths.Count == 0) {
				Terminal.WriteLine("You appear to be trapped.");
				return;
			}

			Terminal.WriteLine("Accessible Locations:");
			foreach (Location.Path p in Player.location.Paths) {
				Terminal.WriteLine($"  {NumericCallNames.HeadingOfAdd(p.location)}{p.location.ListingName}");
			}
		}

		#endregion
	}
}
