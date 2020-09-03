using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RPGCuzWhyNot.Inventory;
using RPGCuzWhyNot.Inventory.Item;

namespace RPGCuzWhyNot {
	public class Location : IThing, IHasItemInventory {
		public string Name { get; }
		public string CallName { get; }
		public readonly string description;
		public readonly string pathDescription;
		public readonly ReadOnlyCollection<Path> Paths;
		public readonly ItemInventory items;
		ItemInventory IHasItemInventory.Inventory => items;
		public string ListingName => ThingExt.ListingName(this);

		private readonly List<Path> paths = new List<Path>();

		public class Path {
			public readonly Location location;
			public readonly string description;

			protected internal Path(Location location, string description) {
				this.location = location;
				this.description = description;
			}
		}

		public Location(string callName, string name, string description) {
			CallName = callName;
			Name = name;
			this.description = description;
			items = new ItemInventory(this);
			Paths = paths.AsReadOnly();
		}

		public void AddPathTo(Location location, string description) {
			if (paths.Any(path => path.location == location)) throw new InvalidOperationException("A path already exists");
			paths.Add(new Path(location, description));
		}

		public bool GetConnectedLocationByCallName(string callName, out Location connectedLocation) {
			foreach (Path path in paths) {
				if (path.location.CallName != callName) continue;

				connectedLocation = path.location;
				return true;
			}

			connectedLocation = default;
			return false;
		}

		public void AddItem(IItem item) {
			if (items.Contains(item))
				throw new InvalidOperationException("Item already added");

			items.MoveItem(item);
		}

		public bool RemoveItem(IItem item) {
			return items.Remove(item);
		}

		public IItem GetItemByCallName(string itemCallName) {
			return items.FirstOrDefault(item => item.CallName == itemCallName);
		}

		public void PrintEnterInformation() {
			string title = $"----- [ {Name} ] -----";

			using (new FGColorScope(ConsoleColor.Yellow)) {
				ConsoleUtils.SlowWriteLine(title);
			}
			PrintInformation();
			using (new FGColorScope(ConsoleColor.Yellow)) {
				ConsoleUtils.SlowWriteLine(new string('-', title.Length));
			}
		}

		public void PrintInformation() {
			ConsoleUtils.SlowWriteLine(description);
			foreach (Path path in paths) {
				ConsoleUtils.SlowWriteLine($"{path.description} [{path.location.CallName}]");
			}

			foreach (IItem item in items) {
				ConsoleUtils.SlowWriteLine($"{item.DescriptionOnGround} [{item.CallName}]");
			}
		}

		public override string ToString() {
			return ListingName;
		}
	}
}

