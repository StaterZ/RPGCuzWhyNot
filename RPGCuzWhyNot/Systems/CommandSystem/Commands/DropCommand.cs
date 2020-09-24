using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class DropCommand : Command {
		public override string[] CallNames { get; } = {"drop"};
		public override string HelpText { get; } = "Drop an item on the current location";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("Drop what?");
				return;
			}

			string callName = args.FirstArgument;
			if ((NumericCallNames.Get(callName, out IItem item) &&
					(item.ContainedInventory == Player.Inventory
						|| item.ContainedInventory == Player.Wielding
						|| item.ContainedInventory == Player.Wearing))
				|| Player.Inventory.ContainsCallName(callName, out item)
				|| Player.Wielding.ContainsCallName(callName, out item)
				|| Player.Wearing.ContainsCallName(callName, out item)) {
				if (Player.location.items.MoveItem(item)) {
					Terminal.WriteLine($"You dropped {item.Name}.");
				} else {
					Terminal.WriteLine($"Couldn't drop {item.Name}.");
				}
			} else {
				Terminal.WriteLine("I don't know what that is.");
			}
		}
	}
}