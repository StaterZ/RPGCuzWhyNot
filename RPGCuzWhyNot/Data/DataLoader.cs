using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Inventory.Item;

// TODO: Add character prototypes when they have been implemented in the rest of the game.

namespace RPGCuzWhyNot.Data {
	public static class DataLoader {
		private const string DataPath = "GameData/";
		private const string LocationsPath = DataPath + "location";
		private const string ItemsPath = DataPath + "item";

		private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions {
			Converters = {new JsonStringEnumConverter()}
		};

		private static Dictionary<string, Prototype> prototypes;
		private static Dictionary<string, Location> locations;

		public static ReadOnlyDictionary<string, Prototype> Prototypes { get; private set; }
		public static ReadOnlyDictionary<string, Location> Locations { get; private set; }

		/// <summary>
		/// Load all the game data files into the registry.
		/// </summary>
		public static void LoadGameData() {
			prototypes = new Dictionary<string, Prototype>();
			Prototypes = new ReadOnlyDictionary<string, Prototype>(prototypes);
			LoadPrototypesFromPath<ItemPrototype>(ItemsPath);
			LoadPrototypesFromPath<LocationPrototype>(LocationsPath);

			ValidatePrototypes();

			locations = new Dictionary<string, Location>();
			Locations = new ReadOnlyDictionary<string, Location>(locations);
			// Create the locations.
			foreach (LocationPrototype prototype in prototypes.Values.OfType<LocationPrototype>()) {
				Location location = prototype.Create();
				locations.Add(prototype.Id, location);
			}

			SetupLocations();
		}

		/// <summary>
		/// Construct an item from the prototype registry.
		/// </summary>
		/// <param name="id">The id of the item</param>
		public static IItem CreateItem(string id) {
			if (prototypes.TryGetValue(id, out Prototype prototype) && prototype is ItemPrototype itemPrototype)
				return itemPrototype.Create();

			throw new Exception($"An item prototype with the id '{id}' was not found.");
		}

		/// <summary>
		/// Get an existing location from the registry.
		/// </summary>
		/// <param name="id">The id of the location.</param>
		public static Location GetLocation(string id) {
			if (!locations.TryGetValue(id, out Location location))
				throw new Exception($"A location with the id '{id} was not found.");

			return location;
		}

		private static void LoadPrototypesFromPath<TProto>(string path) where TProto : Prototype {
			string[] dataFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);

			foreach (string filePath in dataFiles) {
				string fileContent = File.ReadAllText(filePath);
				TProto prototype;
				try {
					prototype = JsonSerializer.Deserialize<TProto>(fileContent, serializerOptions);
				} catch (JsonException e) {
					throw new DataLoaderException($"Failed to deserialize data file: {filePath}.", e);
				}

				// The data file name is used as the item id.
				string id = Path.GetFileNameWithoutExtension(filePath);
				prototype.Id = id;
				if (!prototypes.TryAdd(id, prototype))
					throw new DataLoaderException($"Duplicate prototype definition of '{id}': {filePath}.");
			}
		}

		private static void SetupLocations() {
			foreach ((string id, Location location) in locations) {
				LocationPrototype locationPrototype = location.Prototype;

				// Add all the paths to the location.
				foreach ((string pathName, string pathDescription) in locationPrototype.Paths) {
					if (!locations.TryGetValue(pathName, out Location destination))
						throw new DataLoaderException($"Location '{pathName}' not found. Referenced by '{id}'.");

					location.AddPathTo(destination, pathDescription);
				}

				// Create the items in the location.
				foreach (string itemName in locationPrototype.Items) {
					if (!prototypes.TryGetValue(itemName, out Prototype proto) || !(proto is ItemPrototype item))
						throw new DataLoaderException($"Item '{itemName}' not found. Referenced by '{id}'.");

					location.items.MoveItem(item.Create());
				}
			}
		}

		private static void ValidatePrototypes() {
			foreach (Prototype prototype in prototypes.Values) {
				CheckRequiredProperty(prototype, prototype.CallName, "callName");
				CheckRequiredProperty(prototype, prototype.Name, "name");

				switch (prototype) {
					case LocationPrototype locationPrototype: {
						CheckRequiredProperty(locationPrototype, locationPrototype.Description, "description");

						if (locationPrototype.Paths.ContainsKey(locationPrototype.Id))
							throw new DataLoaderException($"Prototype '{locationPrototype.Id}' contains a path to itself.");
						break;
					}
					case ItemPrototype itemPrototype: {
						CheckRequiredProperty(itemPrototype, itemPrototype.DescriptionInInventory, "inventoryDescription");
						CheckRequiredProperty(itemPrototype, itemPrototype.DescriptionOnGround, "groundDescription");

						if (itemPrototype.IsWearable) {
							if (itemPrototype.CoveredParts == 0) ThrowMissingProperty(itemPrototype, "coveredParts");
							if (itemPrototype.CoveredLayers == 0) ThrowMissingProperty(itemPrototype, "coveredLayers");
						}

						break;
					}
				}
			}
		}

		private static void CheckRequiredProperty(Prototype prototype, string value, string propertyName) {
			if (string.IsNullOrWhiteSpace(value))
				ThrowMissingProperty(prototype, propertyName);
		}

		private static void ThrowMissingProperty(Prototype prototype, string propertyName) {
			throw new DataLoaderException($"Missing property '{propertyName}' in prototype '{prototype.Id}'.");
		}
	}
}