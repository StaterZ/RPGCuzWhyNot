using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class UnwieldCommand : Command {
		public override string[] CallNames { get; } = {"unwield"};
		public override string HelpText { get; } = "Remove something wielded";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("No item specified");
				return;
			}

			string callName = args.FirstArgument;
			if ((NumericCallNames.Get(callName, out IWieldable item) && item.ContainedInventory == Player.Wielding)
			|| Player.Wielding.ContainsCallName(callName, out item)) {
				if (Player.Inventory.MoveItem(item)) {
					Terminal.WriteLine($"You unwield {item.Name} and put it in your inventory.");
				} else {
					Terminal.WriteLine($"Couldn't unwield {item.Name}.");
				}
			} else {
				Terminal.WriteLine("You're wielding nothing such.");
			}
		}
	}
}