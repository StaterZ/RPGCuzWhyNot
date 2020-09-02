using System;
using RPGCuzWhyNot.Inventory;
using RPGCuzWhyNot.Inventory.Item;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public class Player : Character, IHasItemInventory, ICanWear, ICanWield {
		public ItemInventory Inventory { get; }
		public WearablesInventory Wearing { get; }
		public WieldablesInventory Wielding { get; }
		public readonly CommandHandler commandHandler = new CommandHandler();

		public Player() {
			Inventory = new ItemInventory(this);
			Wearing = new WearablesInventory(this);
			Wielding = new WieldablesInventory(this, 2);

			//init health
			health = new Health(100);
			health.OnDamage += ctx => {
				ConsoleUtils.SlowWriteLine($"{ctx.inflictor} hit you for {ctx.Delta} damage");
			};
			health.OnDeath += ctx => {
				ConsoleUtils.SlowWriteLine($"{ctx.inflictor} killed you!");
			};

			LoadCommands();
		}

		private void LoadCommands() {
			commandHandler.AddCommand(new Command(new[] { "go", "goto", "enter" }, "Go to another location", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("Where to?");
				}

				if (location.GetConnectedLocationByCallName(args[1], out Location newLocation)) {
					ConsoleUtils.FakeLoad(1000);
					location = newLocation;
					location.PrintEnterInformation();
				} else {
					ConsoleUtils.SlowWriteLine("I don't know where that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "where" }, "Show information about the current location", args => {
				ConsoleUtils.SlowWriteLine($"You are in: {location}");
				location.PrintInformation();
			}));
			commandHandler.AddCommand(new Command(new[] { "ls", "list", "locations" }, "List all locations accessible from the current one", args => {
				ConsoleUtils.SlowWriteLine("Locations:");
				foreach (Location loc in location.Paths) {
					Console.Write("  ");
					ConsoleUtils.SlowWriteLine(loc.ToString());
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wear" }, "Wear something", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					Wear(item);
				} else {
					ConsoleUtils.SlowWriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwear" }, "Remove something worn", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("No item specified");
					return;
				}

				if (Wearing.ContainsCallname(args[1], out IWearable item)) {
					if (Inventory.MoveItem(item)) {
						ConsoleUtils.SlowWriteLine($"You remove {item.Name} and put it in your inventory");
					}
				} else {
					ConsoleUtils.SlowWriteLine("You're wearing nothing such");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wearing", "armor" }, "List what is currently being worn", args => {
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "wield" }, "Wield something", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					Wield(item);
				} else {
					ConsoleUtils.SlowWriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwield" }, "Remove something wielded", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Wielding.ContainsCallname(callName, out IWieldable item)) {
					if (Inventory.MoveItem(item)) {
						ConsoleUtils.SlowWriteLine($"You unwield {item.Name} and put it in your inventory.");
					} else {
						ConsoleUtils.SlowWriteLine($"Couldn't unwield {item.Name}.");
					}
				} else {
					ConsoleUtils.SlowWriteLine("You're wielding nothing such.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wielding" }, "List what is currently being wielded", args => {
				ListWielding();
			}));
			commandHandler.AddCommand(new Command(new[] { "equip" }, "Equip something, either by wearing it, or wielding it", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					if (item is IWearable) {
						if (item is IWieldable) {
							ConsoleUtils.SlowWriteLine($"That's ambiguous, as {item.Name} can be wielded and worn.");
						} else {
							Wear(item);
						}
					} else if (item is IWieldable) {
						Wield(item);
					} else {
						ConsoleUtils.SlowWriteLine($"{item.Name} is neither wieldable, nor wearable.");
					}
				} else {
					ConsoleUtils.SlowWriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unequip" }, "Remove something worn or wielded", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("No item specified");
					return;
				}

				string callName = args[1];
				bool wielding = ((IInventory)Wielding).ContainsCallName(callName, out IItem item);
				string action = wielding ? "unwield" : "remove";
				if (wielding || ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					if (Inventory.MoveItem(item)) {
						ConsoleUtils.SlowWriteLine($"You {action} {item.Name} and put it in your inventory.");
					} else {
						ConsoleUtils.SlowWriteLine($"Couldn't {action} {item.Name}.");
					}
				} else {
					ConsoleUtils.SlowWriteLine("You have nothing such equiped.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "equipped" }, "List what is currently worn and wielded", args => {
				if (args.Length > 1) {
					ConsoleUtils.SlowWriteLine($"'{args[0]}' does not take any arguments");
					return;
				}
				ListWielding();
				Console.WriteLine();
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "take", "pickup", "grab" }, "Take an item from the current location", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("Take what?");
					return;
				}

				if (location.items.ContainsCallName(args[1], out IItem item)) {
					if (Inventory.MoveItem(item)) {
						ConsoleUtils.SlowWriteLine($"You picked up {item.Name} and put it in your inventory.");
					} else {
						ConsoleUtils.SlowWriteLine($"Couldn't pick up {item.Name}.");
					}
				} else {
					ConsoleUtils.SlowWriteLine("Can't see that here.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "drop" }, "Drop an item on the current location", args => {
				if (args.Length < 2) {
					ConsoleUtils.SlowWriteLine("Drop what?");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item)
				|| ((IInventory)Wielding).ContainsCallName(callName, out item)
				|| ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					if (location.items.MoveItem(item)) {
						ConsoleUtils.SlowWriteLine($"You dropped {item.Name}.");
					} else {
						ConsoleUtils.SlowWriteLine($"Couldn't drop {item.Name}.");
					}
				} else {
					ConsoleUtils.SlowWriteLine("I don't know what that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "inventory" }, "List the items in your inventory", args => {
				if (Inventory.Count <= 0) {
					ConsoleUtils.SlowWriteLine("Your inventory is empty.");
					return;
				}

				ConsoleUtils.SlowWriteLine("Inventory:");
				foreach (IItem item in Inventory) {
					Console.Write("  ");
					ConsoleUtils.SlowWriteLine(item.ListingName());
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "items" }, "List the items nearby", args => {
				if (location.items.Count == 0) {
					ConsoleUtils.SlowWriteLine("You look around but can't find anything of use.");
					return;
				}

				ConsoleUtils.SlowWriteLine("You look around and see:");
				foreach (IItem item in location.items) {
					Console.Write("  ");
					ConsoleUtils.SlowWriteLine(item.ListingName());
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "help", "commands" }, "Show this list", args => {
				ConsoleUtils.SlowWriteLine("Commands:");
				string[] formattedCommandCallNames = new string[commandHandler.commands.Count];
				int longestFormattedCommandCallName = 0;
				for (int i = 0; i < commandHandler.commands.Count; i++) {
					string formattedCommandCallName = ConsoleUtils.StringifyArray("[", ", ", "]", commandHandler.commands[i].callNames);
					formattedCommandCallNames[i] = formattedCommandCallName;

					if (formattedCommandCallName.Length > longestFormattedCommandCallName) {
						longestFormattedCommandCallName = formattedCommandCallName.Length;
					}
				}

				for (int i = 0; i < commandHandler.commands.Count; i++) {
					ConsoleUtils.SlowWriteLine($"{formattedCommandCallNames[i].PadRight(longestFormattedCommandCallName)} - {commandHandler.commands[i].helpText}", 200, 300);
				}
			}));
		}

		public void ReactToCommand(string[] args) {
			if (args.Length == 0) return;

			if (!commandHandler.TryHandle(args)) {
				ConsoleUtils.SlowWriteLine("I don't understand.");
			}
		}

		private string ItemSource(IItem item) {
			if (item.ContainedInventory == Inventory) {
				return " from your inventory";
			} else if (item.ContainedInventory == location.items) {
				return " from the ground";
			} else {
				return string.Empty;
			}
		}

		private void Wear(IThing thing) {
			if (thing is IWearable wearable) {
				if (Wearing.MoveItem(wearable)) {
					int covers = (int)wearable.CoverdParts;
					string target = string.Empty;
					if ((wearable.CoverdParts & BodyParts.Chest) == 0) {
						target = " your " + ((BodyParts)(covers & ~(covers - 1))).ToString().ToLower();
					}

					ConsoleUtils.SlowWriteLine($"You take {wearable.Name}{ItemSource(wearable)} and put it on{target}.");
				}
			} else {
				ConsoleUtils.SlowWriteLine($"{thing.Name} can not be worn.");
			}
		}

		private void Wield(IThing thing) {
			if (thing is IWieldable wieldable) {
				if (Wielding.MoveItem(wieldable)) {
					string handPlural = wieldable.HandsRequired != 1 ? "s" : string.Empty;
					ConsoleUtils.SlowWriteLine($"You take {wieldable.Name}{ItemSource(wieldable)} and wield it in your hand{handPlural}.");
				}
			} else {
				ConsoleUtils.SlowWriteLine($"{thing.Name} can not be wielded.");
			}
		}

		private void ListWearing() {
			if (Wearing.Count == 0) {
				ConsoleUtils.SlowWriteLine("You are not wearing anything");
				return;
			}
			ConsoleUtils.SlowWriteLine("You are currently wearing:");
			foreach (IWearable wearable in Wearing) {
				ConsoleUtils.SlowWriteLine($"  {wearable.ListingWithStats()}");
			}
		}

		private void ListWielding() {
			if (Wielding.Count == 0) {
				ConsoleUtils.SlowWriteLine("You are unarmed.");
				return;
			}
			ConsoleUtils.SlowWriteLine("You are currently wielding:");
			foreach (IWieldable weildable in Wielding) {
				ConsoleUtils.SlowWriteLine($"  {weildable.ListingWithStats()}");
			}
		}
	}
}

