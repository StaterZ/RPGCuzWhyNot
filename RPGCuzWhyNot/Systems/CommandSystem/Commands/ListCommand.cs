using RPGCuzWhyNot.Things;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class ListCommand : Command {
		public override string[] CallNames { get; } = {"ls", "list", "locations"};
		public override string HelpText { get; } = "List all locations accessible from the current one";

		public override void Execute(CommandArguments args) {
			Terminal.WriteLine("Locations:");
			foreach (Location.Path path in Player.location.Paths) {
				Terminal.Write("  ");
				Terminal.WriteLine(path.location.ListingName);
			}
		}
	}
}