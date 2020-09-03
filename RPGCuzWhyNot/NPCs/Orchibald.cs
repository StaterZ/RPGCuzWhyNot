namespace RPGCuzWhyNot.NPCs
{
	public class Orchibald : NPC {
		private const int voiceFrequency = 400;

		public Orchibald() {
			CallName = "orhibald";
			Name = "Orhibald (Smith)";

		}

		public override void Converse(Character character, string response) {
			ConsoleUtils.SlowWrite("Hello", voiceFrequency, 50);
			ConsoleUtils.SlowWriteLine(".....", voiceFrequency, 10);
			ConsoleUtils.Sleep(1000);
			ConsoleUtils.SlowWriteLine("Anyways, i wasn't being suspicous at all just now...", voiceFrequency);
			ConsoleUtils.Sleep(200);
			ConsoleUtils.SlowWriteLine("Just so you know *cough*", voiceFrequency);
		}
	}

	public class SmithyCustomer : NPC {
		private const int voiceFrequency = 1000;

		public SmithyCustomer() {
			CallName = "customer";
			Name = "Unknown Smithy Customer";
		}

		public override void Converse(Character character, string response) {
			ConsoleUtils.SlowWrite("Get away from me you vulgar beast!", voiceFrequency);
		}
	}
}

