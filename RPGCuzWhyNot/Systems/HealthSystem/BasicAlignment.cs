using System.Collections.Generic;

namespace RPGCuzWhyNot.Systems.HealthSystem {
	public class BasicAlignment : Alignment {
		public List<BasicAlignment> enemies;

        public override bool CanHarm(Alignment other) {
			return other is BasicAlignment otherAlignment && enemies.Contains(otherAlignment);
		}
	}
}