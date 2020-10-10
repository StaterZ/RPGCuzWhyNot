using System.Collections;
using System.Collections.Generic;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.AttackSystem;
using RPGCuzWhyNot.Systems.HealthSystem;
using RPGCuzWhyNot.Systems.HealthSystem.Alignments;
using RPGCuzWhyNot.Systems.Inventory;
using RPGCuzWhyNot.Things.Characters.Races;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Things.Characters {
	public abstract class Character : IInflictor, IThing, ICanWear, ICanWield, IHasItemInventory {
		public string Name { get; set; }
		public string CallName { get; set; }
		public string FormattedCallName => $"{{fg:Cyan}}([{CallName}])";

		public ItemInventory Inventory { get; }
		public WearablesInventory Wearing { get; }
		public WieldablesInventory Wielding { get; }

		public Race race;
		public Location location;
		public Health health;
		public IAlignment Alignment { get; set; }
		public Stats stats;

		protected Character(Race race) {
			this.race = race;

			Inventory = new ItemInventory(this);
			Wearing = new WearablesInventory(this);
			Wielding = new WieldablesInventory(this, 2);
		}

		bool IHasInventory.ContainsCallName(string callName, out IItem item) => Inventory.ContainsCallName(callName, out item);
		bool IHasInventory.MoveItem(IItem item, bool silent) => Inventory.MoveItem(item, silent);

		IEnumerator<IItem> IEnumerable<IItem>.GetEnumerator() => Inventory.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Inventory).GetEnumerator();

		public void Say(string message) {
			Terminal.WriteLine($"{Name}: {message}");
		}

		public virtual string ListingName => ThingExt.DefaultListingName(this);

		public abstract void DoTurn(Fight fight);


		public abstract bool WantsToHarm(Character character);
	}
}

