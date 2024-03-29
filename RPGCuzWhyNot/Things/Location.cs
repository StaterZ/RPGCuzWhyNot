using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data.Prototypes;
using RPGCuzWhyNot.Systems.Inventory;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Characters.NPCs;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Things {
	public class Location : IThing, IHasItemInventory {
		public string Name { get; }
		public string CallName { get; }
		public string FormattedCallName => $"{{fg:Yellow}}([{CallName}])";
		public string ListingName => ThingExt.DefaultListingName(this);
		public LocationPrototype Prototype { get; set; }
		public ReadOnlyCollection<Path> Paths { get; }
		public ReadOnlyCollection<CharacterLocationData> Characters { get; }
		ItemInventory IHasItemInventory.Inventory => items;

		public readonly string description;
		public readonly ItemInventory items;
		private readonly List<Path> paths = new List<Path>();
		private readonly List<CharacterLocationData> characters = new List<CharacterLocationData>();

		public class Path {
			public readonly Location location;
			public readonly string description;

			public Path(Location location, string description) {
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

		public void AddPathTo(Location location, string pathDescription) {
			if (paths.Any(path => path.location == location)) throw new InvalidOperationException("A path already exists");
			paths.Add(new Path(location, pathDescription));
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
				Terminal.WriteLine($"{NumericCallNames.HeadingOfAdd(path.location)}{path.description} {path.location.FormattedCallName}");
			}

			foreach (IItem item in items) {
				Terminal.WriteLine($"{NumericCallNames.HeadingOfAdd(item)}{item.DescriptionOnGround} {item.FormattedCallName}");
			}

			foreach (CharacterLocationData characterLocationData in characters) {
				string heading = NumericCallNames.HeadingOfAdd(characterLocationData.character);
				Terminal.WriteLine($"{heading}{characterLocationData.glanceDescription} {characterLocationData.character.FormattedCallName}");
			}
		}

		bool IHasInventory.MoveItem(IItem item, bool silent) => items.MoveItem(item, silent);
		bool IHasInventory.ContainsCallName(string callName, out IItem item) => items.ContainsCallName(callName, out item);

		IEnumerator<IItem> IEnumerable<IItem>.GetEnumerator() => items.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
	}
}

