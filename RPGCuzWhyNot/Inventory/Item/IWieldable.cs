using RPGCuzWhyNot.Inventory;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot {
	public interface IWieldable : IItem {
		int HandsRequired { get; }
		int MeleeDamage { get; }
	}
}

