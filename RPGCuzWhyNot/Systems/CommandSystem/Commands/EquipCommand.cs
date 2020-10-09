using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class EquipCommand : Command {
		public override string[] CallNames { get; } = {"equip"};
		public override string HelpText { get; } = "Equip something, either by wearing it, or wielding it";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("No item specified");
				return;
			}

			string callName = args.FirstArgument;
			if (NumericCallNames.Get(callName, out IItem item)
			|| Program.player.Inventory.ContainsCallName(callName, out item)
			|| Program.player.location.items.ContainsCallName(callName, out item)) {
				if (item is IWearable) {
					if (item is IWieldable) {
						Terminal.WriteLine($"That's ambiguous, as {item.Name} can be wielded and worn.");
					} else {
						PlayerCommands.Wear(item);
					}
				} else if (item is IWieldable) {
					PlayerCommands.Wield(item);
				} else {
					Terminal.WriteLine($"{item.Name} is neither wieldable, nor wearable.");
				}
			} else {
				Terminal.WriteLine("Item not found, does it exist?");
			}
		}
	}
}