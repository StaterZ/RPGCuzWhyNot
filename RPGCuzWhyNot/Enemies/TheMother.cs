using System;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot.Enemies {
	public class TheMother : Character {
		public TheMother() {
			Name = "Din Mamma";
			health = new Health(9001);
			health.OnDeath += Die;
		}

		public override void Attack(Character target) {
			target.health.TakeDamage((float)Math.Ceiling(target.health.CurrentHealth / 2), this);
		}

		private void Die(HealthChangeInfo ctx) {
			Say("NOOOOOO!!!");
		}
	}
}
