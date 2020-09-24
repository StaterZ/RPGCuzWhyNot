using System;

namespace RPGCuzWhyNot.Systems.CommandSystem {
	public class DynamicCommand : Command {
		public override string[] CallNames { get; }
		public override string HelpText { get; }
		public Action<CommandArguments> Effect { get; }
		public override string[] Keywords { get; }

		public DynamicCommand(string[] callNames, string helpText, Action<CommandArguments> effect, string[] keywords = null) {
			CallNames = callNames;
			HelpText = helpText;
			Effect = effect ?? throw new ArgumentNullException(nameof(effect));
			Keywords = keywords ?? Array.Empty<string>();
		}

		public override void Execute(CommandArguments args) {
			Effect.Invoke(args);
		}
	}
}

