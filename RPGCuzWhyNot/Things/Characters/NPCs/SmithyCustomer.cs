using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	public class SmithyCustomer : NPC {
		private const int voiceFrequency = 1000;

		public SmithyCustomer() : base(new Human(Humanoid.Gender.Female)) {
			CallName = "customer";
			Name = "Unknown Smithy Customer";
		}

		public override void Converse(Character character, string response) {
			Terminal.Write("Get away from me you vulgar beast!", voiceFrequency);
		}

		public override void DoTurn(Fight fight) {
			throw new System.NotImplementedException();
		}
	}
}