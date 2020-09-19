using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot.Things.Item {
	public class WieldableItem : SimpleItem, IWieldable {
		public int HandsRequired { get; set; } = 1;
		public Requirements UsageRequirements { get; set; }
		public ItemAction[] ItemActions { get; set; }

		public WieldableItem(string name, string callName, string descInv, string descGrnd = null)
			: base(name, callName, descInv, descGrnd) { }

		public virtual string ListingWithStats => WieldableExt.DefaultListingWithStats(this);

	}
}

