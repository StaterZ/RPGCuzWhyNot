namespace RPGCuzWhyNot.Systems.HealthSystem {
	public abstract class Alignment {
		public abstract bool CanHarm(Alignment other);
	}
}