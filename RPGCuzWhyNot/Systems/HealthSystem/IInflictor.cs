using RPGCuzWhyNot.Systems.HealthSystem.Alignments;

namespace RPGCuzWhyNot.Systems.HealthSystem {
	public interface IInflictor {
		IAlignment Alignment { get; }
	}
}