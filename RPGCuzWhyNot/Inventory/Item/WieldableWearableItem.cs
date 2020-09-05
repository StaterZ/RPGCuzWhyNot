using System.Collections.Generic;

namespace RPGCuzWhyNot.Inventory.Item {
	public class WieldableWearableItem : WearableItem, IWieldable {
		public int HandsRequired { get; set; }
		public IEnumerable<ItemAction> ItemActions { get; set; }

		public WieldableWearableItem(string name, string callName, string descInv, string descGround)
			: base(name, callName, descInv, descGround) { }


		//public override string ListingWithStats
		//	=> ContainedInventory is WieldablesInventory ? WieldableExt.DefaultListingWithStats(this) : WearableExt.DefaultListingWithStats(this);
		public override string ListingWithStats => "broken atm..."; //todo fix this
	}
}

