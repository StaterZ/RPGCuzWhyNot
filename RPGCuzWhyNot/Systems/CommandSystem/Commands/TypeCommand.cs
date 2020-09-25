namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class TypeCommand : Command {
		public override string[] CallNames { get; } = {"type"};
		public override string HelpText { get; } = "Echo whats written";

		public override void Execute(CommandArguments args) {
			Terminal.WriteLine(args.FirstArgument);
		}
	}
}