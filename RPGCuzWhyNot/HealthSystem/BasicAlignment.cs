using System.Collections.Generic;

namespace StaterZ.Core.HealthSystem {
	public class BasicAlignment : Alignment {
		public List<BasicAlignment> enemies;

        public override bool CanHarm(Alignment other) {
			return other is BasicAlignment otherAlignment && enemies.Contains(otherAlignment);
		}
	}
}