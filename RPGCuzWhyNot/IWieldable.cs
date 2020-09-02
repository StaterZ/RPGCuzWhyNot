

namespace RPGCuzWhyNot {
	public interface IWieldable : IItem {
		int HandsRequired { get; }
		int MeleeDamage { get; }
	}
}

