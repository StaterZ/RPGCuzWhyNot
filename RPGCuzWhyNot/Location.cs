using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RPGCuzWhyNot.Data;
using RPGCuzWhyNot.Inventory;
using RPGCuzWhyNot.Inventory.Item;
using RPGCuzWhyNot.NPCs;

namespace RPGCuzWhyNot {
	public class Location : IThing, IHasItemInventory {
		public string Name { get; }
		public string CallName { get; }
		public string FormattedCallName => $"{{fg:Yellow}}([{CallName}])";
		public readonly string description;
		public readonly string pathDescription;
		public readonly ReadOnlyCollection<Path> Paths;
		public readonly ReadOnlyCollection<CharacterLocationData> Characters;
		public readonly ItemInventory items;
		ItemInventory IHasItemInventory.Inventory => items;
		public string ListingName => ThingExt.DefaultListingName(this);
		public LocationPrototype Prototype { get; set; }

		private readonly List<Path> paths = new List<Path>();
		private readonly List<CharacterLocationData> characters = new List<CharacterLocationData>();

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
			Characters = characters.AsReadOnly();
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

		public bool GetCharacterByCallName(string callName, out Character character) {
			foreach (CharacterLocationData characterLocationData in characters) {
				if (characterLocationData.character.CallName != callName) continue;

				character = characterLocationData.character;
				return true;
			}

			character = default;
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
			NumericCallNames.Clear();

			Terminal.WriteLine($"{{fg:Yellow}}({title})");
			PrintInformation();
			Terminal.WriteLine($"{{fg:Yellow}}({new string('-', title.Length)})");
		}

		public void AddNPC(NPC npc, string glanceDescription, string approachDescription) {
			characters.Add(new CharacterLocationData(npc, glanceDescription, approachDescription));
		}

		public void PrintInformation() {
			Terminal.WriteLine(description);
			foreach (Path path in paths) {
				Terminal.WriteLine($"{NumericCallNames.NumberHeading}{path.description} {path.location.FormattedCallName}");
				NumericCallNames.Add(path.location);
			}

			foreach (IItem item in items) {
				Terminal.WriteLine($"{NumericCallNames.NumberHeading}{item.DescriptionOnGround} {item.FormattedCallName}");
				NumericCallNames.Add(item);
			}

			foreach (CharacterLocationData characterLocationData in characters) {
				Terminal.WriteLine($"{NumericCallNames.NumberHeading}{characterLocationData.glanceDescription} {characterLocationData.character.FormattedCallName}");
				NumericCallNames.Add(characterLocationData.character);
			}
		}
	}
}

