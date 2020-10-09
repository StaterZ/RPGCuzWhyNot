using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class ItemsCommand : Command {
		public override string[] CallNames { get; } = {"items"};
		public override string HelpText { get; } = "Inspect an item in your inventory";

		public override void Execute(CommandArguments args) {
			NumericCallNames.Clear();
			PlayerCommands.ListLocationItems();
		}
	}
}