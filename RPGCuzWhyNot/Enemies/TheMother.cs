using System;

namespace RPGCuzWhyNot.Enemies {
	public class TheMother : Character {
		public TheMother() {
			name = "Din Mamma";
			health = 9001;
		}

		protected override int CalculateDamage(Character target) {
			return (int)Math.Ceiling(target.health / 2.0);
		}

		protected override void Die() {
			Say("NOOOOOO!!!");
		}
	}
}
