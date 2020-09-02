using System;
using System.Collections.Generic;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public class Player : Character, IHaveItems, ICanWear {
		public ItemInventory Inventory { get; }
		public WearablesInventory Wearing { get; }
		public readonly CommandHandler commandHandler = new CommandHandler();

		public Player() {
			Inventory = new ItemInventory(this);
			Wearing = new WearablesInventory(this);

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
			commandHandler.AddCommand(new Command(new[] { "wear", "equip" }, "Wear something", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callname = args[1];
				foreach (IItem item in Inventory) {
					if (item.Callname != callname) continue;
					if (item is IWearable w) {
						if (Wearing.MoveItem(w)) {
							Console.WriteLine($"You take {w.Name} from your inventory and put it on");
						}
						return;
					} else {
						Console.WriteLine($"{item.Name} is not wearable");
						return;
					}
				}
				Console.WriteLine("Item not found, does it exist?");
			}));
			commandHandler.AddCommand(new Command(new[] { "unwear" }, "Remove something worn", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
					return;
				}

				string callname = args[1];
				foreach (IWearable piece in Wearing)
					if (piece.Callname == callname) {
						Inventory.MoveTo(piece);
						Console.WriteLine($"You remove {piece.Name} and put it in your inventory");
						return;
					}
				Console.WriteLine("You're wearing nothing such");
			}));
			commandHandler.AddCommand(new Command(new[] { "wearing", "armor" }, "List what is currently being worn", args => {
				if (args.Length > 1) {
					Console.WriteLine($"'{args[0]}' does not take any arguments");
					return;
				}
				if (Wearing.Count == 0) {
					Console.WriteLine("You are not wearing anything");
					return;
				}
				Console.WriteLine("You are currently wearing:");
				foreach (IWearable w in Wearing) {
					Console.WriteLine($"  {w.ListingWithStats()}");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "take", "pickup", "grab" }, "Take an item from the current location", args => {
				if (args.Length < 2) {
					Console.WriteLine("Take what?");
					return;
				}

				if (!TryPickup(args[1])) {
					Console.WriteLine("Can't see that here.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "drop" }, "Drop an item on the current location", args => {
				if (args.Length < 2) {
					Console.WriteLine("Drop what?");
					return;
				}

				string callname = args[1];
				foreach (IItem item in Inventory) {
					if (item.Callname == callname) {
						location.items.MoveTo(item);
						Console.WriteLine($"You dropped {item.Name} in {location.Name}");
						return;
					}
				}
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

		private bool TryPickup(string callName) {
			IItem item = location.GetItemByCallName(callName);

			if (item != null) {
				Inventory.MoveTo(item);
				Console.WriteLine($"You picked up: {item}.");
				return true;
			}

			return false;
		}
	}
}

