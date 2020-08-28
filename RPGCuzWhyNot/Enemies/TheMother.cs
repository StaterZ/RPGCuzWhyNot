using System;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot.Enemies {
	public class TheMother : Character {
		public TheMother() {
			name = "Din Mamma";
			health = new Health(9001);
			health.OnDeath += Die;
		}

		private void Die(HealthChangeInfo ctx) {
			Say("NOOOOOO!!!");
		}
	}
}
