using RPGCuzWhyNot.Races.Humanoids;

namespace RPGCuzWhyNot.NPCs {
	public class SmithyCustomer : NPC {
		private const int voiceFrequency = 1000;

		public SmithyCustomer() : base(new Human(Humanoid.Gender.Female)) {
			CallName = "customer";
			Name = "Unknown Smithy Customer";
		}

		public override void Converse(Character character, string response) {
			Terminal.Write("Get away from me you vulgar beast!", voiceFrequency);
		}

		public override PlanOfAction PlanTurn(params Character[] opponents) {
			throw new System.NotImplementedException();
		}
	}
}