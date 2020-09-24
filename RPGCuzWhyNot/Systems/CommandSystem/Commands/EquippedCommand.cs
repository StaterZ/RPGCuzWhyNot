using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class EquippedCommand : Command {
		public override string[] CallNames { get; } = {"equipped", "gear"};
		public override string HelpText { get; } = "List what is currently worn and wielded";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument != "") {
				Terminal.WriteLine($"'{args.CommandName}' does not take any arguments");
				return;
			}

			NumericCallNames.Clear();
			PlayerCommands.ListWielding();
			Terminal.WriteLine();
			PlayerCommands.ListWearing();

		}
	}
}