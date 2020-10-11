using System;
using System.Collections.Generic;
using System.Linq;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.CommandSystem {
	public class CommandHandler {
		public readonly List<Command> commands = new List<Command>();

		private static readonly char[] commandArgumentSeparators = { ' ' };

		// These contain spaces so that they won't match any callnames or keywords.
		public const string commandName = " cmd";
		public const string firstParameterName = " arg";

		public void AddCommand(Command command) => commands.Add(command);
		public void AddCommands(IEnumerable<Command> commandsToAdd) => commands.AddRange(commandsToAdd);

		public bool TryHandle(string message) {
			string[] parts = message.Split(commandArgumentSeparators, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0) return true;
			Dictionary<string, string> args = new Dictionary<string, string>();
			string paramName = firstParameterName;
			foreach (Command command in commands) {
				if (!command.CallNames.Contains(parts[0])) continue;
				args.Clear();
				args[commandName] = parts[0];
				int argStart = 1;
				for (int i = 1; i < parts.Length; ++i) {
					string part = parts[i];
					if (command.Keywords.Contains(part)) {
						args[paramName] = Utils.StringifyArray("", " ", "", parts, argStart..i);
						argStart = i + 1;
						paramName = part;
					}
				}
				args[paramName] = Utils.StringifyArray("", " ", "", parts, argStart..);
				command.Execute(new CommandArguments(args));
				return true;
			}

			return false;
		}
	}
}

