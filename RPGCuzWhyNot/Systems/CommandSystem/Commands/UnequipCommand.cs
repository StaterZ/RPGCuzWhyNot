using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class UnequipCommand : Command {
		public override string[] CallNames { get; } = {"unequip"};
		public override string HelpText { get; } = "Remove something worn or wielded";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("No item specified");
				return;
			}

			string callName = args.FirstArgument;
			if ((NumericCallNames.Get(callName, out IItem item)
				&& (item.ContainedInventory == Player.Wearing
				|| item.ContainedInventory == Player.Wielding))
			|| Player.Wielding.ContainsCallName(callName, out item)
			|| Player.Wearing.ContainsCallName(callName, out item)) {
				string action = item.ContainedInventory == Player.Wielding ? "unwield" : "remove";
				if (Player.Inventory.MoveItem(item)) {
					Terminal.WriteLine($"You {action} {item.Name} and put it in your inventory.");
				} else {
					Terminal.WriteLine($"Couldn't {action} {item.Name}.");
				}
			} else {
				Terminal.WriteLine("You have nothing such equipped.");
			}
		}
	}
}