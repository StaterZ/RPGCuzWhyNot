using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class ThrowCommand : Command {
		public override string[] CallNames { get; } = {"throw", "yeet"};
		public override string HelpText { get; } = "Throw {darkgray}(something) at {darkgray}(something else)";
		public override string[] Keywords { get; } = {"at"};

		public override void Execute(CommandArguments args) {
			string throwableCallName = args.FirstArgument;
			if (throwableCallName == "") {
				Terminal.WriteLine("You need something to throw.");
				return;
			}

			if (!NumericCallNames.Get(throwableCallName, out IItem throwable)
			&& !Program.player.Inventory.ContainsCallName(throwableCallName, out throwable)) {
				Terminal.WriteLine("I don't understand what you're trying to throw.");
			} else if (!args.Get("at", out string atCallName) || atCallName == "") {
				Terminal.WriteLine($"You need something to throw {throwable.Name} at.");
			} else {
				Terminal.WriteLine($"{{darkgray}}((pretending)) Threw {throwable.Name} at {atCallName}.");
			}
		}
	}
}