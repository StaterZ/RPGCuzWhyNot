using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	public class Orchibald : NPC {
		private const int voiceFrequency = 400;

		public Orchibald() : base(new Dwarf(Humanoid.Gender.Male)) {
			CallName = "orhibald";
			Name = "Orhibald (Smith)";

		}

		public override void Converse(Character character, string response) {
			Terminal.Write("Hello", voiceFrequency, 50);
			Terminal.WriteLine(".....", voiceFrequency, 10);
			ConsoleUtils.Sleep(1000);
			Terminal.WriteLine("Anyways, i wasn't being suspicous at all just now...", voiceFrequency);
			ConsoleUtils.Sleep(200);
			Terminal.WriteLine("Just so you know *cough*", voiceFrequency);
		}

		public override PlanOfAction PlanTurn(params Character[] opponents) {
			throw new System.NotImplementedException();
		}
	}
}

