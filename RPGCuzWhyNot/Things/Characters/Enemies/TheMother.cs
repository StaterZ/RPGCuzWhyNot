using System;
using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;

namespace RPGCuzWhyNot.Things.Characters.Enemies {
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

		public override PlanOfAction PlanTurn(Fight fight) {
			PlanOfAction planOfAction = new PlanOfAction(stats);

			foreach (Character combatant in fight.combatants) {
				if (combatant == this) continue;

				AttackWithDuster(combatant); //temp
			}

			return planOfAction;
		}

		private void Die(HealthChangeInfo ctx) {
			Say("NOOOOOO!!!");
		}
	}
}
