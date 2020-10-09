using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class UnwearCommand : Command {
		public override string[] CallNames { get; } = {"unwear"};
		public override string HelpText { get; } = "Remove something worn";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("No item specified");
				return;
			}

			string callName = args.FirstArgument;
			if ((NumericCallNames.Get(callName, out IWearable item) && item.ContainedInventory == Program.player.Wearing)
			|| Program.player.Wearing.ContainsCallName(callName, out item)) {
				if (Program.player.Inventory.MoveItem(item)) {
					Terminal.WriteLine($"You remove {item.Name} and put it in your inventory");
				}
			} else {
				Terminal.WriteLine("You're wearing nothing such");
			}
		}
	}
}