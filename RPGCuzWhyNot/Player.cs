using System;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public class Player : Character, IHaveItems, ICanWear, ICanWield {
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

				string callname = args[1];
				if (Inventory.ContainsCallname(callname, out IItem item) || location.items.ContainsCallname(callname, out item))
					Wear(item);
				else
					Console.WriteLine("Item not found, does it exist?");
			}));
			commandHandler.AddCommand(new Command(new[] { "unwear" }, "Remove something worn", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				if (Wearing.ContainsCallname(args[1], out IWearable item)) {
					if (Inventory.MoveItem(item))
						Console.WriteLine($"You remove {item.Name} and put it in your inventory");
				} else
					Console.WriteLine("You're wearing nothing such");
			}));
			commandHandler.AddCommand(new Command(new[] { "wearing", "armor" }, "List what is currently being worn", args => {
				if (args.Length > 1) {
					Console.WriteLine($"'{args[0]}' does not take any arguments");
					return;
				}
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "wield" }, "Wield something", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callname = args[1];
				if (Inventory.ContainsCallname(callname, out IItem item) || location.items.ContainsCallname(callname, out item))
					Wield(item);
				else
					Console.WriteLine("Item not found, does it exist?");
			}));
			commandHandler.AddCommand(new Command(new[] { "unwield" }, "Remove something wielded", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callname = args[1];
				if (Wielding.ContainsCallname(callname, out var item))
					if (Inventory.MoveItem(item))
						Console.WriteLine($"You unwield {item.Name} and put it in your inventory.");
					else
						Console.WriteLine($"Couldn't unwield {item.Name}.");
				else
					Console.WriteLine("You're wielding nothing such.");
			}));
			commandHandler.AddCommand(new Command(new[] { "wielding" }, "List what is currently being wielded", args => {
				if (args.Length > 1) {
					Console.WriteLine($"'{args[0]}' does not take any arguments");
					return;
				}
				ListWielding();
			}));
			commandHandler.AddCommand(new Command(new[] { "equip" }, "Equip something, either by wearing it, or wielding it", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callname = args[1];
				if (Inventory.ContainsCallname(callname, out IItem item) || location.items.ContainsCallname(callname, out item))
					if (item is IWearable)
						if (item is IWieldable)
							Console.WriteLine($"That is ambiguous, as {item.Name} is be both wielded and worn.");
						else
							Wear(item);
					else if (item is IWieldable)
						Wield(item);
					else
						Console.WriteLine($"{item.Name} is neither wieldable, nor wearable.");
				else
					Console.WriteLine("Item not found, does it exist?");
			}));
			commandHandler.AddCommand(new Command(new[] { "unequip" }, "Remove something worn or wielded", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callname = args[1];
				bool wielding = ((IInventory)Wielding).ContainsCallname(callname, out IItem item);
				string action = wielding ? "unwield" : "remove";
				if (wielding || ((IInventory)Wearing).ContainsCallname(callname, out item))
					if (Inventory.MoveItem(item))
						Console.WriteLine($"You {action} {item.Name} and put it in your inventory.");
					else
						Console.WriteLine($"Couldn't {action} {item.Name}.");
				else
					Console.WriteLine("You have nothing such equiped.");
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

				if (location.items.ContainsCallname(args[1], out IItem item))
					if (Inventory.MoveItem(item))
						Console.WriteLine($"You picked up {item.Name} and put it in your inventory.");
					else
						Console.WriteLine($"Couldn't pick up {item.Name}.");
				else
					Console.WriteLine("Can't see that here.");
			}));
			commandHandler.AddCommand(new Command(new[] { "drop" }, "Drop an item on the current location", args => {
				if (args.Length < 2) {
					Console.WriteLine("Drop what?");
					return;
				}

				string callname = args[1];
				if (Inventory.ContainsCallname(callname, out IItem item)
				|| ((IInventory)Wielding).ContainsCallname(callname, out item)
				|| ((IInventory)Wearing).ContainsCallname(callname, out item))
					if (location.items.MoveItem(item))
						Console.WriteLine($"You dropped {item.Name} in {location.Name}");
					else
						Console.WriteLine($"Couldn't drop {item.Name}.");
				else
					Console.WriteLine("I don't know what that is.");
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
			if (item.ContainedInventory == Inventory)
				return " from your inventory";
			else if (item.ContainedInventory == location.items)
				return " from the ground";
			else
				return "";
		}

		private void Wear(IItem item) {
			if (item is IWearable w) {
				if (Wearing.MoveItem(w)) {
					uint covers = (uint)w.CoversParts;
					string target = "";
					if ((w.CoversParts & BodyParts.Chest) == 0)
						target = " your " + ((BodyParts)(covers & ~(covers - 1))).ToString().ToLower();
					Console.WriteLine($"You take {w.Name}{ItemSource(w)} and put it on{target}.");
				}
				return;
			} else {
				Console.WriteLine($"{item.Name} can not be worn.");
				return;
			}
		}

		private void Wield(IItem item) {
			if (item is IWieldable w) {
				if (Wielding.MoveItem(w)) {
					string handPlural = w.HandsRequired != 1 ? "s" : "";
					Console.WriteLine($"You take {w.Name}{ItemSource(w)} and wield it in your hand{handPlural}.");
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

