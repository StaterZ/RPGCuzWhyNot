using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Things.Characters;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class AttackCommand : Command {
		public override string[] CallNames => new [] {"attack", "challenge", "confront", "fight"};

		public override string HelpText => "attack someone";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine($"{args.CommandName} who?");
				return;
			}

			if (Program.player.location.GetCharacterByCallName(args.FirstArgument, out Character opponent)) {
				Fight fight = new Fight(Program.player, opponent);
				fight.BeginCombat();
			} else {
				Terminal.WriteLine("There's no one here called that.");
			}
		}
	}
}
