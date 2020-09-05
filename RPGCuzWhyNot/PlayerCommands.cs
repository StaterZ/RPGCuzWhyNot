using System;
using RPGCuzWhyNot.Inventory;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot {
	public class PlayerCommands {
		public readonly Player player;
		public readonly CommandHandler commandHandler = new CommandHandler();

		private ItemInventory Inventory => player.Inventory;
		private WieldablesInventory Wielding => player.Wielding;
		private WearablesInventory Wearing => player.Wearing;
		private Location Location { get => player.location; set => player.location = value; }

		public PlayerCommands(Player player) {
			this.player = player;
		}

		public void LoadCommands() {
			commandHandler.AddCommand(new Command(new[] { "go", "goto", "enter" }, "Go to another location", args => {
				if (args.FirstArgument == "") {
					Terminal.WriteLine("Where to?");
				}

				string callName = args.FirstArgument;
				if (NumericCallNames.Get(callName, out Location newLocation)
				|| Location.GetConnectedLocationByCallName(callName, out newLocation)) {
					//ConsoleUtils.FakeLoad(1000);
					Location = newLocation;
					Location.PrintEnterInformation();
				} else {
					Terminal.WriteLine("I don't know where that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "where", "location" }, "Show information about the current location", args => {
				Terminal.WriteLine($"You are in: {Location.Name}");
				NumericCallNames.Clear();
				Location.PrintInformation();
			}));
			commandHandler.AddCommand(new Command(new[] { "ls", "list", "locations" }, "List all locations accessible from the current one", args => {
				Terminal.WriteLine("Locations:");
				foreach (Location.Path path in Location.Paths) {
					Terminal.Write("  ");
					Terminal.WriteLine(path.location.ListingName);
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wear" }, "Wear something", args => {
				if (args.FirstArgument == "") {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args.FirstArgument;
				if (NumericCallNames.Get(callName, out IItem item)
				|| Inventory.ContainsCallName(callName, out item)
				|| Location.items.ContainsCallName(callName, out item)
				|| ((IInventory)Wielding).ContainsCallName(callName, out item)) {
					Wear(item);
				} else {
					Terminal.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwear" }, "Remove something worn", args => {
				if (args.FirstArgument == "") {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args.FirstArgument;
				if ((NumericCallNames.Get(callName, out IWearable item) && item.ContainedInventory == Wearing)
				|| Wearing.ContainsCallName(callName, out item)) {
					if (Inventory.MoveItem(item)) {
						Terminal.WriteLine($"You remove {item.Name} and put it in your inventory");
					}
				} else {
					Terminal.WriteLine("You're wearing nothing such");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "wearing", "armor" }, "List what is currently being worn", args => {
				NumericCallNames.Clear();
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "wield" }, "Wield something", args => {
				if (args.FirstArgument == "") {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args.FirstArgument;
				if (NumericCallNames.Get(callName, out IItem item)
				|| Inventory.ContainsCallName(callName, out item)
				|| Location.items.ContainsCallName(callName, out item)
				|| ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					Wield(item);
				} else {
					Terminal.WriteLine("Item not found, does it exist?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "unwield" }, "Remove something wielded", args => {
				if (args.FirstArgument == "") {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args.FirstArgument;
				if ((NumericCallNames.Get(callName, out IWieldable item) && item.ContainedInventory == Wielding)
				|| Wielding.ContainsCallName(callName, out item)) {
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
				NumericCallNames.Clear();
				ListWielding();
			}));
			commandHandler.AddCommand(new Command(new[] { "equip" }, "Equip something, either by wearing it, or wielding it", args => {
				if (args.FirstArgument == "") {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args.FirstArgument;
				if (NumericCallNames.Get(callName, out IItem item)
				|| Inventory.ContainsCallName(callName, out item)
				|| Location.items.ContainsCallName(callName, out item)) {
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
				if (args.FirstArgument == "") {
					Terminal.WriteLine("No item specified");
					return;
				}

				string callName = args.FirstArgument;
				if ((NumericCallNames.Get(callName, out IItem item) && (item.ContainedInventory == Wearing || item.ContainedInventory == Wielding))
				|| ((IInventory)Wielding).ContainsCallName(callName, out item)
				|| ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					string action = item.ContainedInventory == Wielding ? "unwield" : "remove";
					if (Inventory.MoveItem(item)) {
						Terminal.WriteLine($"You {action} {item.Name} and put it in your inventory.");
					} else {
						Terminal.WriteLine($"Couldn't {action} {item.Name}.");
					}
				} else {
					Terminal.WriteLine("You have nothing such equipped.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "equipped", "gear" }, "List what is currently worn and wielded", args => {
				if (args.FirstArgument != "") {
					Terminal.WriteLine($"'{args.CommandName}' does not take any arguments");
					return;
				}
				NumericCallNames.Clear();
				ListWielding();
				Terminal.WriteLine();
				ListWearing();
			}));
			commandHandler.AddCommand(new Command(new[] { "take", "pickup", "grab" }, "Take an item from the current location", args => {
				if (args.FirstArgument == "") {
					Terminal.WriteLine("Take what?");
					return;
				}

				string callName = args.FirstArgument;
				if ((NumericCallNames.Get(callName, out IItem item) && item.ContainedInventory == Location.items)
				|| Location.items.ContainsCallName(callName, out item)) {
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
				if (args.FirstArgument == "") {
					Terminal.WriteLine("Drop what?");
					return;
				}

				string callName = args.FirstArgument;
				if ((NumericCallNames.Get(callName, out IItem item) &&
					(item.ContainedInventory == Inventory
					|| item.ContainedInventory == Wielding
					|| item.ContainedInventory == Wearing))
				|| Inventory.ContainsCallName(callName, out item)
				|| ((IInventory)Wielding).ContainsCallName(callName, out item)
				|| ((IInventory)Wearing).ContainsCallName(callName, out item)) {
					if (Location.items.MoveItem(item)) {
						Terminal.WriteLine($"You dropped {item.Name}.");
					} else {
						Terminal.WriteLine($"Couldn't drop {item.Name}.");
					}
				} else {
					Terminal.WriteLine("I don't know what that is.");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "inventory" }, "List the items in your inventory", args => {
				NumericCallNames.Clear();
				ListInventory();
			}));
			commandHandler.AddCommand(new Command(new[] { "items" }, "List the items nearby", args => {
				NumericCallNames.Clear();
				ListLocationItems();
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
				Terminal.PushState();
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
				if (args.FirstArgument == "") {
					Terminal.WriteLine($"{args.CommandName} with who?");
					return;
				}

				string callName = args.FirstArgument;
				if (NumericCallNames.Get(callName, out Character conversationPartner)
				|| Location.GetCharacterByCallName(callName, out conversationPartner)) {
					using (new FGColorScope(ConsoleColor.Cyan)) {
						//ConsoleUtils.PrintDivider('#');
						Terminal.WriteLine($"A conversation with <{conversationPartner.Name}> has begun:");

						throw new NotImplementedException();
					}
				} else {
					Terminal.WriteLine("Who now?");
				}
			}));
			commandHandler.AddCommand(new Command(new[] { "everything" }, "List everything interactible", args => {
				NumericCallNames.Clear();
				ListLocationItems();
				Terminal.WriteLine();
				ListWielding();
				Terminal.WriteLine();
				ListWearing();
				Terminal.WriteLine();
				ListInventory();
				Terminal.WriteLine();
				ListLocations();
			}));
			commandHandler.AddCommand(new Command(new string[] { "throw" }, "Throw {darkgray}(something) at {darkgray}(something else)", args => {
				string throwableCallName = args.FirstArgument;
				if (throwableCallName == "") {
					Terminal.WriteLine("You need something to throw.");
					return;
				}

				if (!NumericCallNames.Get(throwableCallName, out IItem throwable)
				&& !Inventory.ContainsCallName(throwableCallName, out throwable)) {
					Terminal.WriteLine("I don't understand what you're trying to throw.");
				} else if (!args.Get("at", out string atCallName) || atCallName == "") {
					Terminal.WriteLine($"You need something to throw {throwable.Name} at.");
				} else {
					Terminal.WriteLine($"{{darkgray}}((pretenting)) Threw {throwable.Name} at {atCallName}.");
				}
			}, new string[] { "at" }));
			commandHandler.AddCommand(new Command(new string[] { "type" }, "Echo whats written", args => {
				Terminal.WriteLine(args.FirstArgument);
			}));
		}

		public void Handle(string message) {
			if (!commandHandler.TryHandle(message)) {
				Terminal.WriteLine("I don't understand.");
			}
		}

		private string ItemSource(IItem item) {
			if (item.ContainedInventory == Inventory) {
				return " from your inventory";
			} else if (item.ContainedInventory == Location.items) {
				return " from the ground";
			} else {
				return "";
			}
		}

		private void Wear(IThing thing) {
			if (thing is IWearable wearable) {
				string source = ItemSource(wearable);
				if (Wearing.MoveItem(wearable)) {
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

		private void Wield(IThing thing) {
			if (thing is IWieldable wieldable) {
				string source = ItemSource(wieldable);
				if (Wielding.MoveItem(wieldable)) {
					string handPlural = wieldable.HandsRequired != 1 ? "s" : "";
					Terminal.WriteLine($"You take {wieldable.Name}{source} and wield it in your hand{handPlural}.");
				}
			} else {
				Terminal.WriteLine($"{thing.Name} can not be wielded.");
			}
		}

		private void ListInventory() {
			if (Inventory.Count <= 0) {
				Terminal.WriteLine("Your inventory is empty.");
				return;
			}
			Terminal.WriteLine("Your inventory contains:");
			foreach (IItem item in Inventory) {
				Terminal.WriteLine($"  {NumericCallNames.NumberHeading}{item.ListingName}");
				NumericCallNames.Add(item);
			}
		}

		private void ListWearing() {
			if (Wearing.Count == 0) {
				Terminal.WriteLine("You are not wearing anything");
				return;
			}
			Terminal.WriteLine("You are currently wearing:");
			foreach (IWearable wearable in Wearing) {
				Terminal.WriteLine($"  {NumericCallNames.NumberHeading}{wearable.ListingWithStats}");
				NumericCallNames.Add(wearable);
			}
		}

		private void ListWielding() {
			if (Wielding.Count == 0) {
				Terminal.WriteLine("You are unarmed.");
				return;
			}
			Terminal.WriteLine("You are currently wielding:");
			foreach (IWieldable wieldable in Wielding) {
				Terminal.WriteLine($"  {NumericCallNames.NumberHeading}{wieldable.ListingWithStats}");
				NumericCallNames.Add(wieldable);
			}
		}

		private void ListLocationItems() {
			if (Location.items.Count <= 0) {
				Terminal.WriteLine("You look around but can't find anything of use.");
				return;
			}

			Terminal.WriteLine("You look around and see:");
			foreach (IItem item in Location.items) {
				Terminal.WriteLine($"  {NumericCallNames.NumberHeading}{item.ListingName}");
				NumericCallNames.Add(item);
			}
		}

		private void ListLocations() {
			if (Location.Paths.Count == 0) {
				Terminal.WriteLine("You appear to be trapped.");
				return;
			}

			Terminal.WriteLine("Accessible Locations:");
			foreach (Location.Path p in Location.Paths) {
				Terminal.WriteLine($"  {NumericCallNames.NumberHeading}{p.location.ListingName}");
				NumericCallNames.Add(p.location);
			}
		}
	}
}
