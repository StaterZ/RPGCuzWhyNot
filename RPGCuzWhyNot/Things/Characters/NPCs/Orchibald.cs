using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Characters.NPCs
{
	[UniqueNpc("orchibald")]
	public class Orchibald : NPC {
		private const int voiceFrequency = 400;

		public Orchibald() : base(new Dwarf(Humanoid.Gender.Male)) {
			health = new Health(100);
		}

		public override void Converse(Character character, string response) {
			Terminal.Write("Hello", voiceFrequency, 50);
			Terminal.WriteLine(".....", voiceFrequency, 10);
			Utils.Sleep(1000);
			Terminal.WriteLine("Anyways, i wasn't being suspicious at all just now...", voiceFrequency);
			Utils.Sleep(200);
			Terminal.WriteLine("Just so you know *cough*", voiceFrequency);
		}

		public override void DoTurn(Fight fight) {
			foreach (Character combatant in fight.Combatants) {
				if (combatant == this) continue;

				combatant.health.TakeDamage(20, this); //temp
			}
		}

		public override bool WantsToHarm(Character character) {
			return true;
		}
	}
}

