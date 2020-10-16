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
			string[] formattedCommandCallNames = new string[commandHandler.commands.Count];
			int longestFormattedCommandCallName = 0;
			for (int i = 0; i < commandHandler.commands.Count; i++) {
				string formattedCommandCallName = Utils.StringifyArray("[", ", ", "]", commandHandler.commands[i].CallNames);
				formattedCommandCallNames[i] = formattedCommandCallName;

				if (formattedCommandCallName.Length > longestFormattedCommandCallName) {
					longestFormattedCommandCallName = formattedCommandCallName.Length;
				}
			}

			Terminal.PushState();
			Terminal.MillisPerChar = 1000 / 300;
			for (int i = 0; i < commandHandler.commands.Count; i++) {
				Terminal.ForegroundColor = Color.Magenta;
				Terminal.Write(formattedCommandCallNames[i].PadRight(longestFormattedCommandCallName));
				Terminal.ForegroundColor = Color.White;
				Terminal.Write(" - ");
				Terminal.WriteLine(commandHandler.commands[i].HelpText);
			}

			Terminal.PopState();
		}
	}
}