namespace RPGCuzWhyNot.Inventory.Item {
	public class ItemAction {
		public string[] CallNames { get; }
		public string Name { get; }
		public string Description { get; }
		public Requirements Requirements { get; }
		public Effects Effects { get; }

		public ItemAction() { }

		public ItemAction(string[] callNames, string name, string description, Requirements requirements, Effects effects) {
			CallNames = callNames;
			Name = name;
			Description = description;
			Requirements = requirements;
			Effects = effects;
		}

		public void Execute() {
			Terminal.WriteLine("me did le execute");
		}
	}
}