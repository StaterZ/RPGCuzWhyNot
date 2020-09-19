namespace RPGCuzWhyNot.Systems.HealthSystem.Alignments {
	public interface IAlignment {
		bool CanHarm(IAlignment other);
	}
}