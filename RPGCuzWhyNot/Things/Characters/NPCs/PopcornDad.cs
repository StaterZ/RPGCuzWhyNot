using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	[UniqueNpc("popcorn_dad")]
	public class PopcornDad : NPC {
		public PopcornDad() : base(new Human(Humanoid.Gender.Male)) {
		}

		public override void DoTurn(Fight fight) {
			throw new System.NotImplementedException();
		}

		public override bool WantsToHarm(Character character) {
			return true;
		}
	}
}