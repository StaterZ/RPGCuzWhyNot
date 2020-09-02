

namespace RPGCuzWhyNot {
	public class WieldableWearableItem : WearableItem, IWieldable {
		public int HandsRequired { get; set; } = 2;
		public int MeleeDamage { get; set; } = 2;

		public WieldableWearableItem(string name, string callName, string descInv, string descGround)
			: base(name, callName, descInv, descGround) { }
	}
}

