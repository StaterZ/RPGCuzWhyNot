namespace StaterZ.Core.HealthSystem {
	public interface IAlignment {
		bool CanHarm(IAlignment other);
	}
}