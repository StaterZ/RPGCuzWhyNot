using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class TakeCommand : Command {
		public override string[] CallNames { get; } = {"take", "pickup", "grab", "yoink"};
		public override string HelpText { get; } = "Take an item from the current location";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("Take what?");
				return;
			}

			string callName = args.FirstArgument;
			if ((NumericCallNames.Get(callName, out IItem item) && item.ContainedInventory == Player.location.items)
			|| Player.location.items.ContainsCallName(callName, out item)) {
				if (Player.Inventory.MoveItem(item)) {
					Terminal.WriteLine($"You picked up {item.Name} and put it in your inventory.");
				} else {
					Terminal.WriteLine($"Couldn't pick up {item.Name}.");
				}
			} else {
				Terminal.WriteLine("Can't see that here.");
			}
		}
	}
}