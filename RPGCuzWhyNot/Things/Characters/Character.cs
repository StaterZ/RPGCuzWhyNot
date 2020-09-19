using RPGCuzWhyNot.AttackSystem;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Systems.HealthSystem.Alignments;
using RPGCuzWhyNot.Things.Characters.Races;

namespace RPGCuzWhyNot.Things.Characters {
	public abstract class Character : IInflictor, IThing {
		public string Name { get; set; }
		public string CallName { get; set; }
		public string FormattedCallName => $"{{fg:Cyan}}([{CallName}])";
		public Race race;
		public Location location;
		public Health health;
		public IAlignment Alignment { get; set; }
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

