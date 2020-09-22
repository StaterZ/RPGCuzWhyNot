using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Characters.NPCs
{
	[UniqueNpc("orchibald")]
	public class Orchibald : NPC {
		private const int voiceFrequency = 400;

		public override void Converse(Character character, string response) {
			Terminal.Write("Hello", voiceFrequency, 50);
			Terminal.WriteLine(".....", voiceFrequency, 10);
			ConsoleUtils.Sleep(1000);
			Terminal.WriteLine("Anyways, i wasn't being suspicious at all just now...", voiceFrequency);
			ConsoleUtils.Sleep(200);
			Terminal.WriteLine("Just so you know *cough*", voiceFrequency);
		}
	}

	[UniqueNpc("village_smithy_customer")]
	public class SmithyCustomer : NPC {
		private const int voiceFrequency = 1000;

		public override void Converse(Character character, string response) {
			Terminal.Write("Get away from me you vulgar beast!", voiceFrequency);
		}
	}
}

