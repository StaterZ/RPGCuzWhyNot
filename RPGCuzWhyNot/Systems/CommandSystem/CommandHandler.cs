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
						args[paramName] = Stringification.StringifyArray("", " ", "", parts, argStart..i);
						argStart = i + 1;
						paramName = part;
					}
				}
				args[paramName] = Stringification.StringifyArray("", " ", "", parts, argStart..);
				command.Execute(new CommandArguments(args));
				return true;
			}

			return false;
		}


		public void DisplayHelp() {
			string[] formattedCommandCallNames = new string[commands.Count];
			int longestFormattedCommandCallName = 0;
			for (int i = 0; i < commands.Count; i++) {
				string formattedCommandCallName = Stringification.StringifyArray("[", ", ", "]", commands[i].CallNames);
				formattedCommandCallNames[i] = formattedCommandCallName;

				if (formattedCommandCallName.Length > longestFormattedCommandCallName) {
					longestFormattedCommandCallName = formattedCommandCallName.Length;
				}
			}
			using (Terminal.PushState()) {
				Terminal.MillisPerChar = 1000 / 300;
				for (int i = 0; i < commands.Count; i++) {
					Terminal.ForegroundColor = ConsoleColor.Magenta;
					Terminal.Write(formattedCommandCallNames[i].PadRight(longestFormattedCommandCallName));
					Terminal.ForegroundColor = ConsoleColor.White;
					Terminal.Write(" - ");
					Terminal.WriteLine(commands[i].HelpText);
				}
			}
		}

		internal bool TryHandle(object trailingCommand) {
			throw new NotImplementedException();
		}
	}
}

