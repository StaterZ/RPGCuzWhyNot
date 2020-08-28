using System;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public abstract class Character : IInflictor {
		public string name;
		public Race race;
		public Location location;
		public Health health;
		public Alignment alignment { get; set; }

		protected virtual void Attack(Character target) {
		}

		public void Say(string message) {
			Console.WriteLine($"{name}: {message}");
		}
	}
}