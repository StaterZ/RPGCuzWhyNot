using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RPGCuzWhyNot {
	public class Location : IThing, IHaveItems {
		public string Name { get; }
		public string Callname { get; }
		public readonly string description;
		public readonly string pathDescription;
		public readonly ReadOnlyCollection<Location> Paths;
		public readonly ItemInventory items;
		ItemInventory IHaveItems.Inventory => items;

		private readonly List<Location> paths = new List<Location>();

		public Location(string callName, string name, string description, string pathDescription) {
			Callname = callName;
			Name = name;
			this.description = description;
			this.pathDescription = pathDescription;
			items = new ItemInventory(this);
			Paths = paths.AsReadOnly();
		}

		public bool HasPathTo(Location location) {
			return paths.Contains(location);
		}

		public void AddPathTo(Location location) {
			if (paths.Contains(location)) throw new InvalidOperationException("Locations are already connected");

			paths.Add(location);
			location.paths.Add(this);
		}

		public bool GetConnectedLocationByCallName(string callName, out Location connectedLocation) {
			foreach (Location location in paths) {
				if (location.Callname == callName) {
					connectedLocation = location;
					return true;
				}
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
			return items.FirstOrDefault(item => item.Callname == itemCallName);
		}

		public void PrintEnterInformation() {
			string title = $"----- [ {Name} ] -----";
			Console.WriteLine(title);
			PrintInformation();
			Console.WriteLine(new string('-', title.Length));
		}

		public void PrintInformation() {
			Console.WriteLine(description);
			foreach (Location location in paths) {
				Console.WriteLine(location.pathDescription + " [" + location.Callname + "]");
			}

			foreach (IItem item in items) {
				Console.WriteLine($"{item.DescriptionOnGround} [{item.Callname}]");
			}
		}

		public override string ToString() {
			return this.ListingName();
		}
	}
}