namespace RPGCuzWhyNot.Inventory.Item {
	public interface IWieldable : IItem {
		int HandsRequired { get; }
		int MeleeDamage { get; }
	}
}

