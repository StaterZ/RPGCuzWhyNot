using RPGCuzWhyNot.Data;

namespace RPGCuzWhyNot.Inventory.Item {
	public interface IItem : IThing {
		string DescriptionInInventory { get; }
		string DescriptionOnGround { get; }
		IInventory ContainedInventory { get; set; }
		ItemPrototype Prototype { get; set; }
	}
}

