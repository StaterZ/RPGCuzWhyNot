using System.Collections;
using System.Collections.Generic;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Systems.Inventory;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Things.Characters {
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

		bool IHasInventory.ContainsCallName(string callName, out IItem item) => Inventory.ContainsCallName(callName, out item);
		bool IHasInventory.MoveItem(IItem item, bool silent) => Inventory.MoveItem(item, silent);

		IEnumerator<IItem> IEnumerable<IItem>.GetEnumerator() => Inventory.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Inventory).GetEnumerator();
	}
}

