using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Things.Characters.Races;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	[UniqueNpc("rabiesRabbit")]
	public class RabiesRabbit : NPC {
		private const int voiceFrequency = 1000;

		public RabiesRabbit() : base(new Rabbit(Gender.Male)) {
			health = new Health(90);
		}

		public override void Converse(Character character, string response) {
			Terminal.Write("*furius gnawing*", voiceFrequency, 20);
		}

		public override void DoTurn(Fight fight) {
			foreach (Character combatant in fight.Combatants) {
				if (combatant == this) continue;

				combatant.health.TakeDamage(10, this);
			}
		}

		public override bool WantsToHarm(Character character) {
			return true;
		}
	}
}

