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
				Terminal.WriteLine($"{ctx.inflictor} hit you for {ctx.Delta} damage");
			};
			health.OnDeath += ctx => {
				Terminal.WriteLine($"{ctx.inflictor} killed you!");
			};

			LoadCommands();
		}

		private void LoadCommands() {
			commandHandler.AddCommand(new Command(new[] { "go", "goto", "enter" }, "Go to another location", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("Where to?");
				}

				if (location.GetConnectedLocationByCallName(args[1], out Location newLocation)) {
					//ConsoleUtils.FakeLoad(1000);
					location = newLocation;
					location.PrintEnterInformation();
				} else {
					Terminal.WriteLine("I don't know where that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "where" }, "Show information about the current location", args => {
				Terminal.WriteLine($"You are in: {location}");
				location.PrintInformation();
			}));
			commandHandler.AddCommand(new Command(new[] { "ls", "list", "locations" }, "List all locations accessible from the current one", args => {
				Terminal.WriteLine("Locations:");
				foreach (Location.Path path in location.Paths) {
					Terminal.Write("  ");
					Terminal.WriteLine(path.location.ListingName);
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wear" }, "Wear something", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					Wear(item);
				} else {
					Terminal.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwear" }, "Remove something worn", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("No item specified");
					return;
				}

				if (Wearing.ContainsCallName(args[1], out IWearable item)) {
					if (Inventory.MoveItem(item)) {
						Terminal.WriteLine($"You remove {item.Name} and put it in your inventory");
					}
				} else {
					Terminal.WriteLine("You're wearing nothing such");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wearing", "armor" }, "List what is currently being worn", args => {
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "wield" }, "Wield something", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					Wield(item);
				} else {
					Terminal.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwield" }, "Remove something wielded", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Wielding.ContainsCallName(callName, out IWieldable item)) {
					if (Inventory.MoveItem(item)) {
						Terminal.WriteLine($"You unwield {item.Name} and put it in your inventory.");
					} else {
						Terminal.WriteLine($"Couldn't unwield {item.Name}.");
					}
				} else {
					Terminal.WriteLine("You're wielding nothing such.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wielding" }, "List what is currently being wielded", args => {
				ListWielding();
			}));
			commandHandler.AddCommand(new Command(new[] { "equip" }, "Equip something, either by wearing it, or wielding it", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item) || location.items.ContainsCallName(callName, out item)) {
					if (item is IWearable) {
						if (item is IWieldable) {
							Terminal.WriteLine($"That's ambiguous, as {item.Name} can be wielded and worn.");
						} else {
							Wear(item);
						}
					} else if (item is IWieldable) {
						Wield(item);
					} else {
						Terminal.WriteLine($"{item.Name} is neither wieldable, nor wearable.");
					}
				} else {
					Terminal.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unequip" }, "Remove something worn or wielded", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args[1];
				bool wielding = ((IInventory)Wielding).ContainsCallName(callName, out IItem item);
				string action = wielding ? "unwield" : "remove";
				if (wielding || ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					if (Inventory.MoveItem(item)) {
						Terminal.WriteLine($"You {action} {item.Name} and put it in your inventory.");
					} else {
						Terminal.WriteLine($"Couldn't {action} {item.Name}.");
					}
				} else {
					Terminal.WriteLine("You have nothing such equiped.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "equipped" }, "List what is currently worn and wielded", args => {
				if (args.Length > 1) {
					Terminal.WriteLine($"'{args[0]}' does not take any arguments");
					return;
				}
				ListWielding();
				Terminal.WriteLine();
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "take", "pickup", "grab" }, "Take an item from the current location", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("Take what?");
					return;
				}

				if (location.items.ContainsCallName(args[1], out IItem item)) {
					if (Inventory.MoveItem(item)) {
						Terminal.WriteLine($"You picked up {item.Name} and put it in your inventory.");
					} else {
						Terminal.WriteLine($"Couldn't pick up {item.Name}.");
					}
				} else {
					Terminal.WriteLine("Can't see that here.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "drop" }, "Drop an item on the current location", args => {
				if (args.Length < 2) {
					Terminal.WriteLine("Drop what?");
					return;
				}

				string callName = args[1];
				if (Inventory.ContainsCallName(callName, out IItem item)
				|| ((IInventory)Wielding).ContainsCallName(callName, out item)
				|| ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					if (location.items.MoveItem(item)) {
						Terminal.WriteLine($"You dropped {item.Name}.");
					} else {
						Terminal.WriteLine($"Couldn't drop {item.Name}.");
					}
				} else {
					Terminal.WriteLine("I don't know what that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "inventory" }, "List the items in your inventory", args => {
				if (Inventory.Count <= 0) {
					Terminal.WriteLine("Your inventory is empty.");
					return;
				}

				Terminal.WriteLine("Inventory:");
				foreach (IItem item in Inventory) {
					Terminal.Write("  ");
					Terminal.WriteLine(item.ListingName());
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "items" }, "List the items nearby", args => {
				if (location.items.Count <= 0) {
					Terminal.WriteLine("You look around but can't find anything of use.");
					return;
				}

				Terminal.WriteLine("You look around and see:");
				foreach (IItem item in location.items) {
					Terminal.Write("  ");
					Terminal.WriteLine(item.ListingName());
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "help", "commands" }, "Show this list", args => {
				Terminal.WriteLine("Commands:");
				string[] formattedCommandCallNames = new string[commandHandler.commands.Count];
				int longestFormattedCommandCallName = 0;
				for (int i = 0; i < commandHandler.commands.Count; i++) {
					string formattedCommandCallName = Stringification.StringifyArray("[", ", ", "]", commandHandler.commands[i].callNames);
					formattedCommandCallNames[i] = formattedCommandCallName;

					if (formattedCommandCallName.Length > longestFormattedCommandCallName) {
						longestFormattedCommandCallName = formattedCommandCallName.Length;
					}
				}
				Terminal.PushState(Terminal.Save.MillisPerChar | Terminal.Save.ForegroundColor);
				Terminal.MillisPerChar = 1000 / 300;
				for (int i = 0; i < commandHandler.commands.Count; i++) {
					Terminal.ForegroundColor = ConsoleColor.Magenta;
					Terminal.Write(formattedCommandCallNames[i].PadRight(longestFormattedCommandCallName));
					Terminal.ForegroundColor = ConsoleColor.White;
					Terminal.Write(" - ");
					Terminal.WriteLine(commandHandler.commands[i].helpText);
				}
				Terminal.PopState();
			}));
			commandHandler.AddCommand(new Command(new[] { "clear" }, "Clear the console", args => {
				Console.Clear();
			}));
			commandHandler.AddCommand(new Command(new[] { "speak", "talk", "converse" }, "Begin a conversation with someone", args => {
				if (args.Length < 2) {
					Terminal.WriteLine($"{args[0]} with who?");
					return;
				}

				if (location.GetCharacterByCallName(args[1], out Character conversationPartner)) {
					using (new FGColorScope(ConsoleColor.Cyan)) {
						//ConsoleUtils.PrintDivider('#');
						Terminal.WriteLine($"A conversation with <{conversationPartner.Name}> has begun:");

						throw new NotImplementedException();
					}
				} else {
					Terminal.WriteLine("Who now?");
				}
			}));
		}

		public void ReactToCommand(string[] args) {
			if (args.Length == 0) return;

			if (!commandHandler.TryHandle(args)) {
				Terminal.WriteLine("I don't understand.");
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
				string source = ItemSource(wearable);
				if (Wearing.MoveItem(wearable)) {
					int covers = (int)wearable.CoveredParts;
					string target = string.Empty;
					if ((wearable.CoveredParts & WearableSlots.Chest) == 0) {
						target = " your " + ((WearableSlots)(covers & ~(covers - 1))).ToString().ToLower();
					}

					Terminal.WriteLine($"You take {wearable.Name}{source} and put it on{target}.");
				}
			} else {
				Terminal.WriteLine($"{thing.Name} can not be worn.");
			}
		}

		private void Wield(IThing thing) {
			if (thing is IWieldable wieldable) {
				string source = ItemSource(wieldable);
				if (Wielding.MoveItem(wieldable)) {
					string handPlural = wieldable.HandsRequired != 1 ? "s" : string.Empty;
					Terminal.WriteLine($"You take {wieldable.Name}{source} and wield it in your hand{handPlural}.");
				}
			} else {
				Terminal.WriteLine($"{thing.Name} can not be wielded.");
			}
		}

		private void ListWearing() {
			if (Wearing.Count == 0) {
				Terminal.WriteLine("You are not wearing anything");
				return;
			}
			Terminal.WriteLine("You are currently wearing:");
			foreach (IWearable wearable in Wearing) {
				Terminal.WriteLine($"  {wearable.ListingWithStats}");
			}
		}

		private void ListWielding() {
			if (Wielding.Count == 0) {
				Terminal.WriteLine("You are unarmed.");
				return;
			}
			Terminal.WriteLine("You are currently wielding:");
			foreach (IWieldable weildable in Wielding) {
				Terminal.WriteLine($"  {weildable.ListingWithStats}");
			}
		}
	}
}

