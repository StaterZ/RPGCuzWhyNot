using System;

namespace RPGCuzWhyNot {
	public class Command {
		public readonly string[] callNames;
		public readonly string helpText;
		public readonly Action<CommandArguments> effect;
		public readonly string[] keywords;
		private readonly static string[] noKeywords = new string[0];

		public Command(string[] callNames, string helpText, Action<CommandArguments> effect, string[] keywords = null) {
			this.callNames = callNames;
			this.helpText = helpText;
			this.effect = effect;
			this.keywords = keywords ?? noKeywords;
		}
	}
}

