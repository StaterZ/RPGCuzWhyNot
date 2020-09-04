using RPGCuzWhyNot.Inventory;
using StaterZ.Core.HealthSystem;

namespace RPGCuzWhyNot {
	public class Player : Character, IHasItemInventory, ICanWear, ICanWield {
		public ItemInventory Inventory { get; }
		public WearablesInventory Wearing { get; }
		public WieldablesInventory Wielding { get; }

		public Player() {
			Inventory = new ItemInventory(this);
			Wearing = new WearablesInventory(this);
			Wielding = new WieldablesInventory(this, 2);

			//init health
			health = new Health(100);
			health.OnDamage += ctx => {
				Terminal.WriteLine($"{ctx.inflictor} hit you for {ctx.Delta} damage");
			};
			health.OnDeath += ctx => {
				Terminal.WriteLine($"{ctx.inflictor} killed you!");
			};
		}
	}
}

