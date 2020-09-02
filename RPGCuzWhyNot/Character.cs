using System;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public abstract class Character : IInflictor, IThing {
		public string Name { get; set; }
		public string Callname { get; set; }
		public Race race;
		public Location location;
		public Health health;
		public Alignment alignment { get; set; }

		public virtual void Attack(Character target) {
		}

		public void Say(string message) {
			Console.WriteLine($"{Name}: {message}");
		}
	}
}