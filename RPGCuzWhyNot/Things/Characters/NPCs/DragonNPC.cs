using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Things.Characters.Races;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	[UniqueNpc("dragon")]
	public class DragonNPC : NPC {
		private const int voiceFrequency = 200;

		public DragonNPC() : base(new Dragon(Gender.Female)) {
			health = new Health(500);
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