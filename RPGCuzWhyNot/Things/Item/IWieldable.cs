namespace RPGCuzWhyNot.Things.Item {
	public interface IWieldable : IItem {
		int HandsRequired { get; set; }
		int MeleeDamage { get; set; }

		string ListingWithStats { get; }
	}
}

