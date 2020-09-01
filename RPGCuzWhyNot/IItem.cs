

namespace RPGCuzWhyNot {
	public interface IItem : IThing {
		string DescriptionInInventory { get; }
		string DescriptionOnGround { get; }
	}
}

