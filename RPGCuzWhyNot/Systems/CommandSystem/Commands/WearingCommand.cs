using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class WearingCommand : Command {
		public override string[] CallNames { get; } = {"wearing", "armor"};
		public override string HelpText { get; } = "List what is currently being worn";

		public override void Execute(CommandArguments args) {
			NumericCallNames.Clear();
			PlayerCommands.ListWearing();
		}
	}
}