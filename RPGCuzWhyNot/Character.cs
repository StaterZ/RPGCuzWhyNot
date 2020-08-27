namespace RPGCuzWhyNot {
	public abstract class Character {
		public string name;
		public Race race;
		public Location location;
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
				Die();
			}
		}

		protected virtual int CalculateDamage(Character target) {
			// TODO
			return 2;
		}

		protected virtual void Die() { }

		public void Say(string message) {
			Console.WriteLine($"{name}: {message}");
		}
	}
}