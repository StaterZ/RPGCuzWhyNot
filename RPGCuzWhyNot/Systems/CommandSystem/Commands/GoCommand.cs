using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class GoCommand : Command {
		public override string[] CallNames { get; } = {"go", "goto", "enter"};
		public override string HelpText { get; } = "Go to another location";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("Where to?");
			}

			string callName = args.FirstArgument;
			if (NumericCallNames.Get(callName, out Location newLocation)
			|| Player.location.GetConnectedLocationByCallName(callName, out newLocation)) {
				Player.location = newLocation;
				Player.location.PrintEnterInformation();
			} else {
				Terminal.WriteLine("I don't know where that is.");
			}
		}
	}
}