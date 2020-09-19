using System.Collections.Generic;

namespace RPGCuzWhyNot.Inventory.Item {
	public interface IWieldable : IItem {
		int HandsRequired { get; set; }
		string ListingWithStats { get; }
		Requirements UsageRequirements { get; set; }
		ItemAction[] ItemActions { get; set; }
	}
}

