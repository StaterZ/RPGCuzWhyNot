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
			itemPrototypes = LoadPrototypesFromPath<ItemPrototype>(ItemsPath);
			locationPrototypes = LoadPrototypesFromPath<LocationPrototype>(LocationsPath);

			ValidatePrototypes();

			locations = new Dictionary<string, Location>(locationPrototypes.Count);
			foreach ((string id, LocationPrototype prototype) in locationPrototypes) {
				Location location = prototype.Create();
				locations.Add(id, location);
			}

			ItemPrototypes = new ReadOnlyDictionary<string, ItemPrototype>(itemPrototypes);
			LocationPrototypes = new ReadOnlyDictionary<string, LocationPrototype>(locationPrototypes);
			Locations = new ReadOnlyDictionary<string, Location>(locations);

			SetupLocations();
		}

		/// <summary>
		/// Construct an item from the prototype registry.
		/// </summary>
		/// <param name="id">The id of the item</param>
		public static IItem CreateItem(string id) {
			if (!itemPrototypes.TryGetValue(id, out ItemPrototype itemPrototype))
				throw new Exception($"An item prototype with the id '{id}' was not found.");

			return itemPrototype.Create();
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

		private static Dictionary<string, TProto> LoadPrototypesFromPath<TProto>(string path) where TProto : Prototype {
			string[] dataFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
			var prototypes = new Dictionary<string, TProto>(dataFiles.Length);

			foreach (string filePath in dataFiles) {
				string fileContent = File.ReadAllText(filePath);
				TProto prototype;
				try {
					prototype = JsonSerializer.Deserialize<TProto>(fileContent, serializerOptions);
				}
				catch (JsonException e) {
					throw new DataLoaderException($"Failed to deserialize data file: {filePath}.", e);
				}

				// The data file name is used as the item id.
				string id = Path.GetFileNameWithoutExtension(filePath);
				prototype.Id = id;
				if (!prototypes.TryAdd(id, prototype))
					throw new DataLoaderException($"Duplicate prototype definition of '{id}': {filePath}.");
			}

			return prototypes;
		}

		private static void SetupLocations() {
			foreach ((string id, Location location) in locations) {
				LocationPrototype locationPrototype = locationPrototypes[id];

				// Add all the paths to the location.
				foreach ((string pathName, string pathDescription) in locationPrototype.Paths) {
					if (!locations.TryGetValue(pathName, out Location destination))
						throw new DataLoaderException($"Location '{pathName}' not found. Referenced by '{id}'.");

					location.AddPathTo(destination, pathDescription);
				}

				// Create the items in the location.
				foreach (string itemName in locationPrototype.Items) {
					if (!itemPrototypes.TryGetValue(itemName, out ItemPrototype item))
						throw new DataLoaderException($"Item '{itemName}' not found. Referenced by '{id}'.");

					location.items.MoveItem(item.Create());
				}
			}
		}

		private static void ValidatePrototypes() {
			static void CheckThingPrototype(Prototype prototype) {
				CheckRequiredProperty(prototype, prototype.CallName, "callName");
				CheckRequiredProperty(prototype, prototype.Name, "name");
			}

			// Validate location prototypes.
			foreach (LocationPrototype proto in locationPrototypes.Values) {
				CheckThingPrototype(proto);
				CheckRequiredProperty(proto, proto.Description, "description");

				if (proto.Paths.ContainsKey(proto.Id))
					throw new DataLoaderException($"Prototype '{proto.Id}' contains a path to itself.");
			}

			// Validate item prototypes.
			foreach (ItemPrototype proto in itemPrototypes.Values) {
				CheckThingPrototype(proto);
				CheckRequiredProperty(proto, proto.DescriptionInInventory, "inventoryDescription");
				CheckRequiredProperty(proto, proto.DescriptionOnGround, "groundDescription");

				if (proto.Wearable) {
					if (proto.CoveredParts == 0) ThrowMissingProperty(proto, "coveredParts");
					if (proto.CoveredLayers == 0) ThrowMissingProperty(proto, "coveredLayers");
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