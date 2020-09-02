namespace RPGCuzWhyNot.Inventory {
	public class WieldableWearableItem : WearableItem, IWieldable {
		public int HandsRequired { get; set; }
		public int MeleeDamage { get; set; }

		public WieldableWearableItem(string name, string callName, string descInv, string descGround)
			: base(name, callName, descInv, descGround) { }
	}
}

