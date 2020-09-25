using RPGCuzWhyNot.Systems.Inventory;

namespace RPGCuzWhyNot.Things.Item {
	public class WieldableWearableItem : WearableItem, IWieldable {
		public int HandsRequired { get; set; }
		public Requirements UsageRequirements { get; set; }
		public ItemAction[] ItemActions { get; set; }

		public WieldableWearableItem(string name, string callName, string descInv, string descGround)
			: base(name, callName, descInv, descGround) { }

		public override string ListingWithStats
			=> ContainedInventory is WieldablesInventory ? WieldableExt.DefaultListingWithStats(this) : WearableExt.DefaultListingWithStats(this);
	}
}

