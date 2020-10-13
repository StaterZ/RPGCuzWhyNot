using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class AttackCommand : Command {
		public override string[] CallNames => new [] {"attack", "challenge", "confront", "fight"};

		public override string HelpText => "attack someone";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine($"{args.CommandName} who?");
				return;
			}

			string callName = args.FirstArgument;
			Character opponent;
			if (NumericCallNames.Get(callName, out opponent) && opponent.location == Program.player.location || Program.player.location.GetCharacterByCallName(callName, out opponent)) {
				Fight fight = new Fight(Program.player, opponent);
				fight.BeginCombat();
			} else {
				Terminal.WriteLine("There's no one here called that.");
			}
		}
	}
}
