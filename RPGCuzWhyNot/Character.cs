using System;
using RPGCuzWhyNot.Races;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public abstract class Character : IInflictor, IThing {
		public string Name { get; set; }
		public string CallName { get; set; }
		public Race race;
		public Location location;
		public Health health;
		public Alignment Alignment { get; set; }

		public virtual void Attack(Character target) {
		}

		public void Say(string message) {
			ConsoleUtils.SlowWriteLine($"{Name}: {message}");
		}
	}
}