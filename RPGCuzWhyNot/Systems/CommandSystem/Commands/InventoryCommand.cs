using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class InventoryCommand : Command {
		public override string[] CallNames { get; } = {"inventory"};
		public override string HelpText { get; } = "List the items in your inventory";

		public override void Execute(CommandArguments args) {
			NumericCallNames.Clear();
			PlayerCommands.ListInventory();
		}
	}
}