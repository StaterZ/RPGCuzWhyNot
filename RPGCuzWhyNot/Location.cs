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
		public readonly ReadOnlyCollection<Location> Paths;
		public readonly ItemInventory items;
		ItemInventory IHasItemInventory.Inventory => items;

		private readonly List<Location> paths = new List<Location>();

		public Location(string callName, string name, string description, string pathDescription) {
			CallName = callName;
			Name = name;
			this.description = description;
			this.pathDescription = pathDescription;
			items = new ItemInventory(this);
			Paths = paths.AsReadOnly();
		}

		public void AddPathTo(Location location) {
			if (paths.Contains(location)) throw new InvalidOperationException("Locations are already connected");

			paths.Add(location);
			location.paths.Add(this);
		}

		public bool GetConnectedLocationByCallName(string callName, out Location connectedLocation) {
			foreach (Location location in paths) {
				if (location.CallName == callName) {
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
			return items.FirstOrDefault(item => item.CallName == itemCallName);
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
				Console.WriteLine(location.pathDescription + " [" + location.CallName + "]");
			}

			foreach (IItem item in items) {
				Console.WriteLine($"{item.DescriptionOnGround} [{item.CallName}]");
			}
		}

		public override string ToString() {
			return this.ListingName();
		}
	}
}