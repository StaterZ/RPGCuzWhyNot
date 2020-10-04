using System;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class QuitCommand : Command {
		public override string[] CallNames { get; } = {":q!"};
		public override string HelpText { get; } = "Quit the game";

		public override void Execute(CommandArguments args) {
			Environment.Exit(0);
		}
	}
}