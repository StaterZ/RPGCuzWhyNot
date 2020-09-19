using System;
using RPGCuzWhyNot.Races.Humanoids;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot.Enemies {
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

		public override PlanOfAction PlanTurn(params Character[] opponents) {
			PlanOfAction planOfAction = new PlanOfAction(stats);

			foreach (Character opponent in opponents) {
				AttackWithDuster(opponent);
			}

			return planOfAction;
		}

		private void Die(HealthChangeInfo ctx) {
			Say("NOOOOOO!!!");
		}
	}
}
