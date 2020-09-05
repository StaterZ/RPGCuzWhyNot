using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.Races;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public abstract class Character : IInflictor, IThing {
		public string Name { get; set; }
		public string CallName { get; set; }
		public string FormattedCallName => $"{{fg:Cyan}}([{CallName}])";
		public Race race;
		public Location location;
		public Health health;
		public Alignment Alignment { get; set; }
		public Stats stats;

		protected Character(Race race) {
			this.race = race;
		}

		public void Say(string message) {
			Terminal.WriteLine($"{Name}: {message}");
		}

		public virtual string ListingName => ThingExt.DefaultListingName(this);

		public abstract PlanOfAction PlanTurn(params Character[] opponents);
	}
}

