namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class ClearCommand : Command {
		public override string[] CallNames { get; } = {"clear"};
		public override string HelpText { get; } = "Clear the console";

		public override void Execute(CommandArguments args) {
			Terminal.Clear();
		}
	}
}