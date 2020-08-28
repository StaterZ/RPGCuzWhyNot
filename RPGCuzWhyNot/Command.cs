using System;

namespace RPGCuzWhyNot {
	public class Command {
		public readonly string[] callNames;
		public readonly string helpText;
		public readonly Action<string[]> execute;

		public Command(string[] callNames, string helpText, Action<string[]> execute) {
			this.callNames = callNames;
			this.helpText = helpText;
			this.execute = execute;
		}
	}
}
