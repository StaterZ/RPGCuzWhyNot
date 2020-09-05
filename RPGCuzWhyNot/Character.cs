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
		public virtual string ListingName => ThingExt.DefaultListingName(this);

		public virtual void Attack(Character target) {
		}

		public void Say(string message) {
			Terminal.WriteLine($"{Name}: {message}");
		}
	}
}

