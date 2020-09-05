namespace RPGCuzWhyNot.Inventory.Item {
	public class ItemAction {
		public readonly string[] callNames;
		public readonly string name;
		public readonly string description;
		public readonly Requirements requirements;
		public readonly Effects effects;

		public ItemAction(string[] callNames, string name, string description, Requirements requirements, Effects effects) {
			this.callNames = callNames;
			this.name = name;
			this.description = description;
			this.requirements = requirements;
			this.effects = effects;
		}

		public void Execute() {
			Terminal.WriteLine("me did le execute");
		}
	}
}