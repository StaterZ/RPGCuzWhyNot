using System;

namespace RPGCuzWhyNot {
	public class Character {
		public string name;
		public Gender gender;
		public int health;

		public void Attack(Character target) {
			int damage = CalculateDamage(target);
			target.TakeDamage(damage, target);
		}

		public virtual void TakeDamage(int damage, Character source) {
			damage = Math.Min(health, damage);
			health -= damage;

			Console.WriteLine($"{source.name} hit {name} for {damage} damage.");

			if (health <= 0) {
				Console.WriteLine($"{name} got killed by {source.name}.");
				// TODO: Die
			}
		}

		protected virtual int CalculateDamage(Character target) {
			// TODO
			return 2;
		}
	}
}