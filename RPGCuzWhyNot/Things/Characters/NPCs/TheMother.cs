using System;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	public class TheMother : Character {
		public TheMother() : base(new Human(Humanoid.Gender.Female)) {
			Name = "Din Mamma";
			health = new Health(9001);
			health.OnDeath += Die;
		}

		//todo: make this an item instead
		private void AttackWithDuster(Character target) {
			target.health.TakeDamage((int)Math.Ceiling(target.health.CurrentHealth / 2f), this);
		}

		public override void DoTurn(Fight fight) {
			//TurnActions planOfAction = new TurnActions(stats); //use later to ensure we're within budget

			foreach (Character combatant in fight.Combatants) {
				if (combatant == this) continue;

				AttackWithDuster(combatant); //temp
			}
		}

		private void Die(HealthChangeInfo ctx) {
			Say("NOOOOOO!!!");
		}

		public override bool WantsToHarm(Character character) {
			return true;
		}
	}
}
