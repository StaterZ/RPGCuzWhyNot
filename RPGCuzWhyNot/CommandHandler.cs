using System.Collections.Generic;
using System.Linq;

namespace RPGCuzWhyNot {
	public class CommandHandler {
		public readonly List<Command> commands = new List<Command>();

		public void AddCommand(Command command) => commands.Add(command);

		public bool TryHandle(string[] args) {
			foreach (Command command in commands) {
				if (!command.callNames.Contains(args[0])) continue;
				command.execute(args);
				return true;
			}

			return false;
		}
	}
}