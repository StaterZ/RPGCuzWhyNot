using System;

namespace RPGCuzWhyNot {
	public class Player : Character {
		public int health;

		public override void TakeDamage(int damage, Character source) {
			damage = Math.Min(health, damage);
			health -= damage;

			Console.WriteLine($"{source.name} hit you for {damage} damage.");

			if (health <= 0) {
				Console.WriteLine($"{source.name} killed you.");
				// TODO: Die
			}
		}
	}
}