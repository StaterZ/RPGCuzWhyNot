using System;
using System.Collections.Generic;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public class Player : Character {
		public readonly List<IItem> inventory = new List<IItem>();
		public readonly CommandHandler commandHandler = new CommandHandler();

		public Player() {
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
			commandHandler.AddCommand(new Command(new[] { "equip" }, "Equip an item", args => {
				if (args.Length < 2) {
					Console.WriteLine("No item specified");
				}

				throw new NotImplementedException();

				//Todo: use args[1] to get the item
				//Item item = null;
				//if (item != null) {
				//	if (TryEquip(item)) {
				//		Console.WriteLine("success");
				//	}
				//} else {
				//	Console.WriteLine("Item not found, does it exist?");
				//}
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
			commandHandler.AddCommand(new Command(new[] { "inventory" }, "List the items in the inventory", args => {
				if (inventory.Count <= 0) {
					Console.WriteLine("Your inventory is empty.");
				}

				Console.WriteLine("Inventory:");
				foreach (IItem item in inventory) {
					Console.Write("  ");
					Console.WriteLine(item);
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "items" }, "List the items nearby", args => {
				if (location.Items.Count == 0) {
					Console.WriteLine("You look around but can't find anything of use.");
				}

				Console.WriteLine("You look around and see:");
				foreach (IItem item in location.Items) {
					Console.Write("  ");
					Console.WriteLine(item);
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

		private bool TryEquip(IItem item) {
			throw new NotImplementedException();
		}

		private bool TryGoto(Location newLocation) {
			if (location.HasPathTo(newLocation)) {
				location = newLocation;
				return true;
			}

			return false;
		}

		private bool TryPickup(string callName) {
			IItem item = location.GetItemByCallName(callName);

			if (item != null) {
				location.RemoveItem(item);
				inventory.Add(item);
				Console.WriteLine($"You picked up: {item}.");
				return true;
			}

			return false;
		}
	}
}