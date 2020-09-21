using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.Data.Prototypes;
using RPGCuzWhyNot.Systems.Inventory;

namespace RPGCuzWhyNot.Things.Item {
	public interface IItem : IThing {
		string DescriptionInInventory { get; }
		string DescriptionOnGround { get; }
		IInventory ContainedInventory { get; set; }
		ItemPrototype Prototype { get; set; }
	}
}

