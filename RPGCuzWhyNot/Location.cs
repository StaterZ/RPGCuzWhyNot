﻿using System;
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

		public bool AddPathTo(Location location) {
			if (paths.Contains(location))
				return false;

			paths.Add(location);
			location.paths.Add(this);
			return true;
		}

		public bool GetConnectedLocationByCallName(string callName, out Location connectedLocation) {
			foreach (Location location in paths) {
				if (location.CallName != callName) continue;

				connectedLocation = location;
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
			foreach (Location location in paths) {
				ConsoleUtils.SlowWriteLine($"{location.pathDescription} [{location.CallName}]");
			}

			foreach (IItem item in items) {
				ConsoleUtils.SlowWriteLine($"{item.DescriptionOnGround} [{item.CallName}]");
			}
		}

		public override string ToString() {
			return ListingName;
		}

		public string ListingName => ThingExt.ListingName(this);
	}
}

