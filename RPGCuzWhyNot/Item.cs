namespace RPGCuzWhyNot {
	public class Item {
		public readonly string name;
		public readonly string callName;
		public readonly string description;

		public Item(string name, string callName, string description) {
			this.name = name;
			this.callName = callName;
			this.description = description;
		}

		public override string ToString() {
			return $"{name} [{callName}]";
		}
	}
}