namespace RPGCuzWhyNot.Things.Item {
	public class WieldableItem : SimpleItem, IWieldable {
		public int HandsRequired { get; set; } = 1;
		public int MeleeDamage { get; set; } = 2;

		public WieldableItem(string name, string callName, string descInv, string descGrnd = null)
			: base(name, callName, descInv, descGrnd) { }

		public virtual string ListingWithStats => WieldableExt.DefaultListingWithStats(this);
	}
}

