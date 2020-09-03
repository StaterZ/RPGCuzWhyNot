using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Inventory.Item;

// TODO: Validate prototypes when loading.
// TODO: Add character prototypes when they have been implemented in the rest of the game.

namespace RPGCuzWhyNot.Data {
	public static class DataLoader {
		private const string DataPath = "GameData/";
		private const string LocationsPath = DataPath + "location";
		private const string ItemsPath = DataPath + "item";

		private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions {
			Converters = {new JsonStringEnumConverter()}
		};

		private static Dictionary<string, ItemPrototype> itemPrototypes;
		private static Dictionary<string, LocationPrototype> locationPrototypes;
		private static Dictionary<string, Location> locations;

		public static ReadOnlyDictionary<string, ItemPrototype> ItemPrototypes { get; private set; }
		public static ReadOnlyDictionary<string, LocationPrototype> LocationPrototypes { get; private set; }
		public static ReadOnlyDictionary<string, Location> Locations { get; private set; }

		/// <summary>
		/// Load all the game data files into the registry.
		/// </summary>
		public static void LoadGameData() {
			itemPrototypes = LoadPrototypesFromPath<ItemPrototype>(ItemsPath)
				.ToDictionary(proto => proto.CallName);
			locationPrototypes = LoadPrototypesFromPath<LocationPrototype>(LocationsPath)
				.ToDictionary(proto => proto.CallName);

			locations = locationPrototypes.Values
				.Select(proto => proto.Create())
				.ToDictionary(location => location.CallName);

			ItemPrototypes = new ReadOnlyDictionary<string, ItemPrototype>(itemPrototypes);
			LocationPrototypes = new ReadOnlyDictionary<string, LocationPrototype>(locationPrototypes);
			Locations = new ReadOnlyDictionary<string, Location>(locations);

			SetupLocations();
		}

		/// <summary>
		/// Construct an item from the prototype registry.
		/// </summary>
		/// <param name="name">The call name of the item</param>
		public static IItem CreateItem(string name) {
			if (!itemPrototypes.TryGetValue(name, out ItemPrototype itemPrototype))
				throw new Exception($"An item prototype with the name '{name}' was not found.");

			return itemPrototype.Create();
		}

		/// <summary>
		/// Get an existing location from the registry.
		/// </summary>
		/// <param name="name">The call name of the location.</param>
		public static Location GetLocation(string name) {
			if (!locations.TryGetValue(name, out Location location))
				throw new Exception($"A location with the name '{name} was not found.");

			return location;
		}

		private static List<TProto> LoadPrototypesFromPath<TProto>(string path) where TProto : Prototype {
			string[] dataFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
			List<TProto> prototypes = new List<TProto>(dataFiles.Length);

			foreach (string filePath in dataFiles) {
				string fileContent = File.ReadAllText(filePath);
				TProto prototype;
				try {
					prototype = JsonSerializer.Deserialize<TProto>(fileContent, serializerOptions);
				}
				catch (JsonException e) {
					throw new Exception($"Failed to deserialize data file: {filePath}", e);
				}

				prototype.CallName = Path.GetFileNameWithoutExtension(filePath);
				prototypes.Add(prototype);
			}

			return prototypes;
		}

		private static void SetupLocations() {
			foreach ((string name, Location location) in locations) {
				LocationPrototype locationPrototype = locationPrototypes[name];

				// Add all the paths to the location.
				foreach ((string pathName, string pathDescription) in locationPrototype.Paths) {
					if (!locations.TryGetValue(pathName, out Location destination))
						throw new Exception($"Referenced location '{pathName}' not found");

					location.AddPathTo(destination, pathDescription);
				}

				// Create the items in the location.
				foreach (string itemName in locationPrototype.Items) {
					if (!itemPrototypes.TryGetValue(itemName, out ItemPrototype item))
						throw new Exception($"Referenced item '{itemName}' not found");

					location.items.MoveItem(item.Create());
				}
			}
		}
	}
}