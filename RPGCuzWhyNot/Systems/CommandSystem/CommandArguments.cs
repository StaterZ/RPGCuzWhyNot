using System.Collections.Generic;

namespace RPGCuzWhyNot.Systems.CommandSystem {
	public struct CommandArguments {
		private readonly Dictionary<string, string> args;

		public readonly string CommandName { get; }
		public readonly string FirstArgument { get; }

		public CommandArguments(Dictionary<string, string> args) {
			this.args = args;
			CommandName = args[CommandHandler.commandName];
			FirstArgument = args[CommandHandler.firstParameterName];
		}

		public bool Get(string keyword, out string callName) => args.TryGetValue(keyword, out callName);
	}
}

