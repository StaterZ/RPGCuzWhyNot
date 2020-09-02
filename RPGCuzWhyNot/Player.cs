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
				Console.WriteLine($"{ctx.inflictor} hit you for {ctx.Delta} damage");
			};
			health.OnDeath += ctx => {
				Console.WriteLine($"{ctx.inflictor} killed you!");
			};

			//init commands
			commandHandler.AddCommand(new Command(new[] { "go", "goto", "enter" }, "Go to another location", args => {
				if (args.Length < 2) {
					Console.WriteLine("Where to?");
				}

				if (location.GetConnectedLocationByCallName(args[1], out Location newLocation)) {
					location = newLocation;
					location.PrintEnterInformation();
				} else {
					Console.WriteLine("I don't know where that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "where" }, "Show information about the current location", args => {
				Console.WriteLine($"You are in: {location}");
				location.PrintInformation();
			}));
			commandHandler.AddCommand(new Command(new[] { "ls", "list", "locations" }, "List all locations accessible from the current one", args => {
				Console.WriteLine("Locations:");
				foreach (Location loc in location.Paths) {
					Console.Write("  ");
					Console.WriteLine(loc);
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wear" }, "Wear something", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					Wear(item);
				} else {
					Console.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwear" }, "Remove something worn", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				if (Wearing.ContainsCallname(args[1], out IWearable item)) {
					if (Inventory.MoveItem(item)) {
						Console.WriteLine($"You remove {item.Name} and put it in your inventory");
					}
				} else {
					Console.WriteLine("You're wearing nothing such");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wearing", "armor" }, "List what is currently being worn", args => {
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "wield" }, "Wield something", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					Wield(item);
				} else {
					Console.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwield" }, "Remove something wielded", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Wielding.ContainsCallname(callName, out IWieldable item)) {
					if (Inventory.MoveItem(item)) {
						Console.WriteLine($"You unwield {item.Name} and put it in your inventory.");
					} else {
						Console.WriteLine($"Couldn't unwield {item.Name}.");
					}
				} else {
					Console.WriteLine("You're wielding nothing such.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wielding" }, "List what is currently being wielded", args => {
				ListWielding();
			}));
			commandHandler.AddCommand(new Command(new[] { "equip" }, "Equip something, either by wearing it, or wielding it", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					if (item is IWearable) {
						if (item is IWieldable) {
							Console.WriteLine($"That's ambiguous, as {item.Name} can be wielded and worn.");
						} else {
							Wear(item);
						}
					} else if (item is IWieldable) {
						Wield(item);
					} else {
						Console.WriteLine($"{item.Name} is neither wieldable, nor wearable.");
					}
				} else {
					Console.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unequip" }, "Remove something worn or wielded", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				bool wielding = ((IInventory)Wielding).ContainsCallName(callName, out IItem item);
				string action = wielding ? "unwield" : "remove";
				if (wielding || ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					if (Inventory.MoveItem(item)) {
						Console.WriteLine($"You {action} {item.Name} and put it in your inventory.");
					} else {
						Console.WriteLine($"Couldn't {action} {item.Name}.");
					}
				} else {
					Console.WriteLine("You have nothing such equiped.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "equipped" }, "List what is currently worn and wielded", args => {
				if (args.Length > 1) {
					Console.WriteLine($"'{args[0]}' does not take any arguments");
					return;
				}
				ListWielding();
				Console.WriteLine();
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "take", "pickup", "grab" }, "Take an item from the current location", args => {
				if (args.Length < 2) {
					Console.WriteLine("Take what?");
					return;
				}

				if (location.items.ContainsCallName(args[1], out IItem item)) {
					if (Inventory.MoveItem(item)) {
						Console.WriteLine($"You picked up {item.Name} and put it in your inventory.");
					} else {
						Console.WriteLine($"Couldn't pick up {item.Name}.");
					}
				} else {
					Console.WriteLine("Can't see that here.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "drop" }, "Drop an item on the current location", args => {
				if (args.Length < 2) {
					Console.WriteLine("Drop what?");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item)
				|| ((IInventory)Wielding).ContainsCallName(callName, out item)
				|| ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					if (location.items.MoveItem(item)) {
						Console.WriteLine($"You dropped {item.Name}.");
					} else {
						Console.WriteLine($"Couldn't drop {item.Name}.");
					}
				} else {
					Console.WriteLine("I don't know what that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "inventory" }, "List the items in your inventory", args => {
				if (Inventory.Count <= 0) {
					Console.WriteLine("Your inventory is empty.");
					return;
				}

				Console.WriteLine("Inventory:");
				foreach (IItem item in Inventory) {
					Console.Write("  ");
					Console.WriteLine(item.ListingName());
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "items" }, "List the items nearby", args => {
				if (location.items.Count == 0) {
					Console.WriteLine("You look around but can't find anything of use.");
					return;
				}

				Console.WriteLine("You look around and see:");
				foreach (IItem item in location.items) {
					Console.Write("  ");
					Console.WriteLine(item.ListingName());
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "help", "commands" }, "Show this list", args => {
				Console.WriteLine("Commands:");
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
					Console.WriteLine($"{formattedCommandCallNames[i].PadRight(longestFormattedCommandCallName)} - {commandHandler.commands[i].helpText}");
				}
			}));
		}

		public void ReactToCommand(string[] args) {
			if (args.Length == 0) return;

			if (!commandHandler.TryHandle(args)) {
				Console.WriteLine("I don't understand.");
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

		private void Wear(IItem item) {
			if (item is IWearable wearable) {
				if (Wearing.MoveItem(wearable)) {
					int covers = (int)wearable.CoverdParts;
					string target = string.Empty;
					if ((wearable.CoverdParts & BodyParts.Chest) == 0) {
						target = " your " + ((BodyParts)(covers & ~(covers - 1))).ToString().ToLower();
					}

					Console.WriteLine($"You take {wearable.Name}{ItemSource(wearable)} and put it on{target}.");
				}
			} else {
				Console.WriteLine($"{item.Name} can not be worn.");
			}
		}

		private void Wield(IItem item) {
			if (item is IWieldable wieldable) {
				if (Wielding.MoveItem(wieldable)) {
					string handPlural = wieldable.HandsRequired != 1 ? "s" : string.Empty;
					Console.WriteLine($"You take {wieldable.Name}{ItemSource(wieldable)} and wield it in your hand{handPlural}.");
				}
			} else {
				Console.WriteLine($"{item.Name} can not be wielded.");
			}
		}

		private void ListWearing() {
			if (Wearing.Count == 0) {
				Console.WriteLine("You are not wearing anything");
				return;
			}
			Console.WriteLine("You are currently wearing:");
			foreach (IWearable w in Wearing) {
				Console.WriteLine($"  {w.ListingWithStats()}");
			}
		}

		private void ListWielding() {
			if (Wielding.Count == 0) {
				Console.WriteLine("You are unarmed.");
				return;
			}
			Console.WriteLine("You are currently wielding:");
			foreach (IWieldable w in Wielding) {
				Console.WriteLine($"  {w.ListingWithStats()}");
			}
		}
	}
}

