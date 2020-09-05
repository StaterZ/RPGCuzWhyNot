namespace RPGCuzWhyNot.Inventory.Item {
	public interface IItemWithInventory : IItem, IHasItemInventory {
		public Fraction WeightFraction { get; set; }
	}
}
