using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems.Inventory;

namespace RPGCuzWhyNot.Things.Item {
	public interface IItemWithInventory : IItem, IHasItemInventory {
		public Fraction WeightFraction { get; set; }
	}
}
