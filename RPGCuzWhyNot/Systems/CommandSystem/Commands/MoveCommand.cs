using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class MoveCommand : Command {
		public override string[] CallNames { get; } = {"move"};
		public override string HelpText { get; } = "Move items between inventories";
		public override string[] Keywords { get; } = {"from", "to"};

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine("What are you trying to move?");
				return;
			}

			string movedCallName = args.FirstArgument;
			IItem itemToMove;
			string source = "";

			if (args.Get("from", out string fromCallName)) {
				if (fromCallName == "") {
					Terminal.WriteLine("What are you trying to move from?");
					return;
				}

				if ((NumericCallNames.Get(fromCallName, out IItemWithInventory fromItem)
					&& (fromItem.ContainedInventory == Program.player.Wearing
					|| fromItem.ContainedInventory == Program.player.Wielding))
				|| Program.player.Wielding.ContainsCallName(fromCallName, out fromItem)
				|| Program.player.Wearing.ContainsCallName(fromCallName, out fromItem)) {
					source = $" from {fromItem.Name}";
				} else {
					Terminal.WriteLine("I don't understand what you're trying to move from.");
					return;
				}

				if (!fromItem.ContainsCallName(movedCallName, out itemToMove)) {
					Terminal.WriteLine("I don't understand what you're trying to move.");
					return;
				}
			} else {
				if (!(NumericCallNames.Get(movedCallName, out itemToMove)
					&& (!itemToMove.IsInsideItemWithInventory(out IItemWithInventory parent)
					|| parent.ContainedInventory == Program.player.Wearing
					|| parent.ContainedInventory == Program.player.Wielding))
				&& !Program.player.Wielding.ContainsCallName(movedCallName, out itemToMove)
				&& !Program.player.Wearing.ContainsCallName(movedCallName, out itemToMove)
				&& !Program.player.location.items.ContainsCallName(movedCallName, out itemToMove)
				&& !Program.player.Inventory.ContainsCallName(movedCallName, out itemToMove)) {
					Terminal.WriteLine("I don't understand what you're trying to move.");
					return;
				}
			}

			string destination = "";
			bool success;

			if (args.Get("to", out string toCallName)) {
				if ((NumericCallNames.Get(toCallName, out IItemWithInventory toItem)
					&& (toItem.ContainedInventory == Program.player.Wearing
					|| toItem.ContainedInventory == Program.player.Wielding))
				|| Program.player.Wielding.ContainsCallName(toCallName, out toItem)
				|| Program.player.Wearing.ContainsCallName(toCallName, out toItem)) {
					destination = $" to {toItem.Name}";
					success = toItem.MoveItem(itemToMove);
				} else {
					Terminal.WriteLine("I don't get where you're trying to move to.");
					return;
				}
			} else {
				if (itemToMove.ContainedInventory == Program.player.Inventory || !itemToMove.IsInsideItemWithInventory()) {
					Terminal.WriteLine("Try specifying where you're trying to move to.");
					return;
				} else {
					success = Program.player.Inventory.MoveItem(itemToMove);
					destination = " to your inventory";
				}
			}

			if (success) {
				Terminal.WriteLine($"Moved {itemToMove.Name}{source}{destination}.");
			}
		}
	}
}