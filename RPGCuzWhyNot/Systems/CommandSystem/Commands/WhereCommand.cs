using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class WhereCommand : Command {
		public override string[] CallNames { get; } = {"where", "location"};
		public override string HelpText { get; } = "Show information about the current location";

		public override void Execute(CommandArguments args) {
			Terminal.WriteLine($"You are in: {Program.player.location.Name}");
			NumericCallNames.Clear();
			Program.player.location.PrintInformation();
		}
	}
}