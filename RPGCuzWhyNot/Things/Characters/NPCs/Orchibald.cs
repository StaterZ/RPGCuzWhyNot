using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Characters.NPCs
{
	public class Orchibald : NPC {
		private const int voiceFrequency = 400;

		public Orchibald() {
			CallName = "orchibald";
			Name = "Orchibald (Smith)";
		}

		public override void Converse(Character character, string response) {
			Terminal.Write("Hello", voiceFrequency, 50);
			Terminal.WriteLine(".....", voiceFrequency, 10);
			ConsoleUtils.Sleep(1000);
			Terminal.WriteLine("Anyways, i wasn't being suspicious at all just now...", voiceFrequency);
			ConsoleUtils.Sleep(200);
			Terminal.WriteLine("Just so you know *cough*", voiceFrequency);
		}
	}

	public class SmithyCustomer : NPC {
		private const int voiceFrequency = 1000;

		public SmithyCustomer() {
			CallName = "customer";
			Name = "Unknown Smithy Customer";
		}

		public override void Converse(Character character, string response) {
			Terminal.Write("Get away from me you vulgar beast!", voiceFrequency);
		}
	}
}

