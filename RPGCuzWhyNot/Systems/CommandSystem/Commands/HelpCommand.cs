using System;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class HelpCommand : Command {
		public override string[] CallNames { get; } = {"help", "commands"};
		public override string HelpText { get; } = "Show this list";

		private readonly CommandHandler commandHandler;

		public HelpCommand(CommandHandler commandHandler) {
			this.commandHandler = commandHandler;
		}

		public override void Execute(CommandArguments args) {
			Terminal.WriteLine("Commands:");
			string[] formattedCommandCallNamesArray = new string[commandHandler.commands.Count];
			int longestFormattedCommandCallNames = 0;
			for (int i = 0; i < commandHandler.commands.Count; i++) {
				string formattedCommandCallNames = commandHandler.commands[i].CallNames.Stringify("[", ", ", "]");
				formattedCommandCallNamesArray[i] = formattedCommandCallNames;

				if (formattedCommandCallNames.Length > longestFormattedCommandCallNames) {
					longestFormattedCommandCallNames = formattedCommandCallNames.Length;
				}
			}

			Terminal.PushState();
			Terminal.MillisPerChar = 1000 / 300;
			for (int i = 0; i < commandHandler.commands.Count; i++) {
				Terminal.ForegroundColor = Color.Magenta;
				Terminal.Write(formattedCommandCallNamesArray[i].PadRight(longestFormattedCommandCallNames));
				Terminal.ForegroundColor = Color.White;
				Terminal.Write(" - ");
				Terminal.WriteLine(commandHandler.commands[i].HelpText);
			}

			Terminal.PopState();
		}
	}
}