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
				&& (item.ContainedInventory == Program.player.Wearing
				|| item.ContainedInventory == Program.player.Wielding))
			|| Program.player.Wielding.ContainsCallName(callName, out item)
			|| Program.player.Wearing.ContainsCallName(callName, out item)) {
				string action = item.ContainedInventory == Program.player.Wielding ? "unwield" : "remove";
				if (Program.player.Inventory.MoveItem(item)) {
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