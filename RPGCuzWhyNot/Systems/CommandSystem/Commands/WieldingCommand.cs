using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class WieldingCommand : Command {
		public override string[] CallNames { get; } = {"wielding"};
		public override string HelpText { get; } = "List what is currently being wielded";

		public override void Execute(CommandArguments args) {
			NumericCallNames.Clear();
			PlayerCommands.ListWielding();
		}
	}
}