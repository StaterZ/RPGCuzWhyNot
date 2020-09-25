using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class EverythingCommand : Command {
		public override string[] CallNames { get; } = {"everything"};
		public override string HelpText { get; } = "List everything interactible";

		public override void Execute(CommandArguments args) {
			NumericCallNames.Clear();
			PlayerCommands.ListLocationItems();
			Terminal.WriteLine();
			PlayerCommands.ListWielding();
			Terminal.WriteLine();
			PlayerCommands.ListWearing();
			Terminal.WriteLine();
			PlayerCommands.ListInventory();
			Terminal.WriteLine();
			PlayerCommands.ListLocations();
		}
	}
}