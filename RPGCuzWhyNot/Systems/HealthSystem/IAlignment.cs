namespace RPGCuzWhyNot.Systems.HealthSystem {
	public interface IAlignment {
		bool CanHarm(IAlignment other);
	}
}