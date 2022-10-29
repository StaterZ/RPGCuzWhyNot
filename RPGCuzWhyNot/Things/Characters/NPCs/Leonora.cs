using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Things.Characters.Races;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	[UniqueNpc("leonora")]
	public class Leonora : NPC {
		private const int voiceFrequency = 200;

		public Leonora() : base(new Dragon(Gender.Female)) {
			health = new Health(1250);
		}

		public override void Converse(Character character, string response) {
			Terminal.Write("*magestic roar*", voiceFrequency, 20);
		}

		public override void DoTurn(Fight fight) {
			foreach (Character combatant in fight.Combatants) {
				if (combatant == this) continue;

				combatant.health.TakeDamage(50, this);
			}
		}

		public override bool WantsToHarm(Character character) {
			return true;
		}
	}
}