using System;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Characters;

namespace RPGCuzWhyNot.Systems.CommandSystem.Commands {
	public class SpeakCommand : Command {
		public override string[] CallNames { get; } = {"speak", "talk", "converse"};
		public override string HelpText { get; } = "Begin a conversation with someone";

		public override void Execute(CommandArguments args) {
			if (args.FirstArgument == "") {
				Terminal.WriteLine($"{args.CommandName} with who?");
				return;
			}

			string callName = args.FirstArgument;
			if (NumericCallNames.Get(callName, out Character conversationPartner)
			|| Player.location.GetCharacterByCallName(callName, out conversationPartner)) {
				Terminal.WriteLine($"{{fg:Cyan}}(A conversation with [{conversationPartner.Name}] has begun:)");

				throw new NotImplementedException();
			} else {
				Terminal.WriteLine("Who now?");
			}
		}
	}
}