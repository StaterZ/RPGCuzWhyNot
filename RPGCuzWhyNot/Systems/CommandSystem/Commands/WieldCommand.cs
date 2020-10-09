using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class WieldCommand : Command {
		public override string[] CallNames { get; } = {"wield"};
		public override string HelpText { get; } = "Wield something";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("No item specified");
				return;
			}

			string callName = args.FirstArgument;
			if (NumericCallNames.Get(callName, out IItem item)
			|| Program.player.Inventory.ContainsCallName(callName, out item)
			|| Program.player.location.items.ContainsCallName(callName, out item)
			|| Program.player.Wearing.ContainsCallName(callName, out item)) {
				PlayerCommands.Wield(item);
			} else {
				Terminal.WriteLine("Item not found, does it exist?");
			}
		}
	}
}