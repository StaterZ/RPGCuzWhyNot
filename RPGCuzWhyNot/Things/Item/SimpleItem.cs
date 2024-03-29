using RPGCuzWhyNot.Systems.Data.Prototypes;
using RPGCuzWhyNot.Systems.Inventory;

namespace RPGCuzWhyNot.Things.Item {
	public class SimpleItem : IItem {
		public string Name { get; }
		public string CallName { get; }
		public string FormattedCallName => $"{{fg:Magenta}}([{CallName}])";
		public string DescriptionInInventory { get; }
		public string DescriptionOnGround { get; }
		public IInventory ContainedInventory { get; set; }
		public ItemPrototype Prototype { get; set; }
		public ItemAction[] ItemActions { get; set; }
		public virtual string ListingName => ThingExt.DefaultListingName(this);

		public SimpleItem(string name, string callName, string descriptionInInventory, string descriptionOnGround = default) {
			Name = name;
			CallName = callName;
			DescriptionInInventory = descriptionInInventory;
			DescriptionOnGround = descriptionOnGround ?? descriptionInInventory;
		}
	}
}

