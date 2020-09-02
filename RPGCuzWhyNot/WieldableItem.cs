

namespace RPGCuzWhyNot {
	public class WieldableItem : SimpleItem, IWieldable {
		public int HandsRequired { get; set; } = 1;
		public int MeleeDamage { get; set; } = 2;

		public WieldableItem(string name, string callname, string descInv, string descGrnd = null)
			: base(name, callname, descInv, descGrnd) { }
	}
}

