using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RPGCuzWhyNot {
	public class Location {
		public readonly string name;
		public readonly string callName;
		public readonly string description;
		public readonly string pathDescription;
		public readonly ReadOnlyCollection<Location> Paths;
		public readonly ReadOnlyCollection<Item> Items;

		private readonly List<Location> paths = new List<Location>();
		private readonly List<Item> items = new List<Item>();

		public Location(string callName, string name, string description, string pathDescription) {
	        this.callName = callName;
			this.name = name;
			this.description = description;
			this.pathDescription = pathDescription;

			Paths = paths.AsReadOnly();
			Items = items.AsReadOnly();
		}

        public bool HasPathTo(Location location) {
			return paths.Contains(location);
		}

        public void AddPathTo(Location location) {
			if (paths.Contains(location)) throw new InvalidOperationException("Locations are already connected");

			paths.Add(location);
			location.paths.Add(this);
        }

        public void AddItem(Item item) {
	        if (items.Contains(item))
		        throw new InvalidOperationException("Item already added");

	        items.Add(item);
        }

        public bool RemoveItem(Item item) {
	        return items.Remove(item);
        }

        public Item GetItemByCallName(string itemCallName) {
	        return items.FirstOrDefault(item => item.callName == itemCallName);
        }

        public void PrintEnterInformation() {
	        string title = $"----- [ {name} ] -----";
	        Console.WriteLine(title);
	        PrintInformation();
	        Console.WriteLine(new string('-', title.Length));
        }

        public void PrintInformation() {
	        Console.WriteLine(description);
	        foreach (Location location in paths) {
		        Console.WriteLine(location.pathDescription + " [" + location.callName + "]");
	        }

	        foreach (Item item in items) {
		        Console.WriteLine($"{item.description} [{item.callName}]");
	        }
        }

        public override string ToString() {
	        return $"{name} [{callName}]";
        }
	}
}