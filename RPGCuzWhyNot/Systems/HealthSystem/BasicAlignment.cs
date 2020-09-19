using System.Collections.Generic;

namespace RPGCuzWhyNot.Systems.HealthSystem {
	public class BasicAlignment : IAlignment {
		public readonly List<IAlignment> alliances = new List<IAlignment>();
		public bool allowInfighting;

		public bool CanHarm(IAlignment other) {
			if (Equals(other)) {
				return allowInfighting;
			} else {
				return !alliances.Contains(other);
			}
		}
	}
}