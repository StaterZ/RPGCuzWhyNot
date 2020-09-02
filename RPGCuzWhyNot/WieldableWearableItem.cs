

namespace RPGCuzWhyNot {
	public class WieldableWearableItem : WearableItem, IWieldable {
		public int HandsRequired { get; set; } = 2;
		public int MeleeDamage { get; set; } = 2;

		public WieldableWearableItem(string name, string callname, string descInv, string descGround)
			: base(name, callname, descInv, descGround) { }
	}
}

