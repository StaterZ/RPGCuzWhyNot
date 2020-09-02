using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Inventory {
	public class SimpleItem : IItem {
		public string Name { get; }
		public string CallName { get; }
		public string DescriptionInInventory { get; }
		public string DescriptionOnGround { get; }
		public IInventory ContainedInventory { get; set; }

		public SimpleItem(string name, string callName, string descriptionInInventory, string descriptionOnGround = default) {
			Name = name;
			CallName = callName;
			DescriptionInInventory = descriptionInInventory;
			DescriptionOnGround = descriptionOnGround ?? descriptionInInventory;
		}

		public override string ToString() => this.ListingName();
	}
}

