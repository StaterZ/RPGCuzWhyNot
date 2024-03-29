using System.Linq;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class InspectCommand : Command {
		public override string[] CallNames { get; } = {"inspect"};
		public override string HelpText { get; } = "Inspect an item in your inventory";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("Inspect what?");
				return;
			}

			string callName = args.FirstArgument;
			Character locationCharacter = null;
			Location connectedLocation = null;
			if (NumericCallNames.Get(callName, out IThing thing)
			|| Program.player.Inventory.ContainsCallName(callName, out thing)
			|| Program.player.Wearing.ContainsCallName(callName, out thing)
			|| Program.player.Wielding.ContainsCallName(callName, out thing)
			|| Program.player.location.items.ContainsCallName(callName, out thing)
			|| Program.player.location.GetCharacterByCallName(callName, out locationCharacter)
			|| Program.player.location.GetConnectedLocationByCallName(callName, out connectedLocation)) {
				Terminal.WriteLine((connectedLocation ?? locationCharacter ?? thing) switch
				{
					IItem item when item.ContainedInventory == Program.player.location.items => item.DescriptionOnGround,
					IItem item => item.DescriptionInInventory,
					Location location when Program.player.location == location => location.description,
					Location location => Program.player.location.Paths
						.FirstOrDefault(a => a.location == location)?.description ?? "I don't know where that is.",
					Character character => character.location.Characters
						.FirstOrDefault(a => a.character == character)?.glanceDescription ?? "I don't know who that is.",
					_ => "I'm not sure what to say..."
				});

				if (thing is IItemWithInventory itemWithInventory) {
					NumericCallNames.Clear();
					PlayerCommands.ListItemWithInventory("", "", itemWithInventory);
				}
			} else {
				Terminal.WriteLine("I don't know what that is.");
			}
		}
	}
}