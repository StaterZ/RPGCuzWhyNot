using System;

namespace RPGCuzWhyNot.Systems.CommandSystem {
	public abstract class Command {
		public abstract string[] CallNames { get; }
		public abstract string HelpText { get; }
		public virtual string[] Keywords => Array.Empty<string>();

		public abstract void Execute(CommandArguments args);
	}
}