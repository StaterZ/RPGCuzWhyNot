

namespace RPGCuzWhyNot {
	public class SimpleItem : IItem {
		public string Name { get; }
		public string Callname { get; }
		public string DescriptionInInventory { get; }
		public string DescriptionOnGround { get; }

		public SimpleItem(string name, string callName, string descriptionInInventory, string descriptionOnGround = default) {
			Name = name;
			Callname = callName;
			DescriptionInInventory = descriptionInInventory;
			DescriptionOnGround = descriptionOnGround ?? descriptionInInventory;
		}

		public override string ToString() {
			return $"{Name} [{Callname}]";
		}
	}
}

