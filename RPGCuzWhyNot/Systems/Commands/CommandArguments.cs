using System.Collections.Generic;
using System.Linq;

namespace RPGCuzWhyNot.Systems.Commands {
	public struct CommandArguments {
		private readonly Dictionary<string, string> args;

		public readonly string CommandName { get; }
		public readonly string FirstArgument { get; }
		public string TrailingCommand { get; }

		public CommandArguments(Dictionary<string, string> args) {
			this.args = args;
			CommandName = args[CommandHandler.commandName];
			FirstArgument = args[CommandHandler.firstParameterName];

			//todo: make the trailing commands not use the dictionary as it does not guarrantee that the items are in the right order
			//for now it seemes to work and i don't wanna mess with sallads command parser thing
			TrailingCommand = args.Aggregate("", (acc, arg) => {
				if (arg.Key != CommandHandler.commandName) {
					if (acc.Length > 0) {
						acc += " ";
					}
					acc += arg.Value;
				}

				return acc;
			});
		}

		public bool Get(string keyword, out string callName) => args.TryGetValue(keyword, out callName);
	}
}

