using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Item;

// TODO: Add character prototypes when they have been implemented in the rest of the game.

namespace RPGCuzWhyNot.Systems.Data {
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

		private static bool loadError;

		/// <summary>
		/// Load all the game data files into the registry.
		/// </summary>
		public static bool LoadGameData() {
			loadError = false;

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

			return !loadError;
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
				// Deserialize the prototype.
				TProto prototype;
				try {
					string fileContent = File.ReadAllText(filePath);
					prototype = JsonSerializer.Deserialize<TProto>(fileContent, serializerOptions);
				}
				catch (IOException e) {
					Error($"Failed to read file \"{filePath}\":\n{e.Message}");
					continue;
				}
				catch (JsonException e) {
					Error($"Failed to deserialize file \"{filePath}\":\n{e.Message}");
					continue;
				}

				((IOnDeserialized)prototype).OnDeserialized();

				// The data file name is used as the item id.
				string id = Path.GetFileNameWithoutExtension(filePath);
				prototype.Id = id;
				if (!prototypes.TryAdd(id, prototype))
					Error($"Duplicate prototype definition of '{id}': \"{filePath}\".");
			}
		}

		private static void SetupLocations() {
			foreach ((string id, Location location) in locations) {
				LocationPrototype locationPrototype = location.Prototype;

				// Add all the paths to the location.
				foreach ((string pathName, string pathDescription) in locationPrototype.Paths) {
					if (!locations.TryGetValue(pathName, out Location destination)) {
						Error($"Location '{pathName}' not found. Referenced by location '{id}'.");
						continue;
					}

					location.AddPathTo(destination, pathDescription);
				}

				// Create the items in the location.
				foreach (string itemName in locationPrototype.Items) {
					if (!prototypes.TryGetValue(itemName, out Prototype proto) || !(proto is ItemPrototype item)) {
						Error($"Item '{itemName}' not found. Referenced by location '{id}'.");
						continue;
					}

					location.items.MoveItem(item.Create());
				}
			}
		}

		private static void ValidatePrototypes() {
			foreach (Prototype proto in prototypes.Values) {
				if (proto.CallName == null) MissingPropertyError(proto, "callName");
				if (proto.Name == null) MissingPropertyError(proto, "name");

				switch (proto) {
					case LocationPrototype locationPrototype:
						ValidateLocationPrototype(locationPrototype);
						break;

					case ItemPrototype itemPrototype:
						ValidateItemPrototype(itemPrototype);
						break;
				}
			}
		}

		private static void ValidateLocationPrototype(LocationPrototype proto) {
			if (proto.Description == null) MissingPropertyError(proto, "description");

			if (proto.Paths.Count == 0)
				LogWarning($"Location '{proto.Id}' has no paths.");

			if (proto.Paths.ContainsKey(proto.Id))
				Error($"Location '{proto.Id}' contains a path to itself.");
		}

		private static void ValidateItemPrototype(ItemPrototype proto) {
			if (proto.DescriptionInInventory == null) MissingPropertyError(proto, "inventoryDescription");
			if (proto.DescriptionOnGround == null) MissingPropertyError(proto, "groundDescription");

			if (proto.IsWieldable) {
				if (proto.HandsRequired == null) MissingPropertyError(proto, "handsRequired");
				if (proto.MeleeDamage == null) MissingPropertyError(proto, "meleeDamage");
			}

			if (proto.IsWearable) {
				if (proto.Defense == null) MissingPropertyError(proto, "defense");
				if (proto.CoveredParts == 0) MissingPropertyError(proto, "coveredParts");
				if (proto.CoveredLayers == 0) MissingPropertyError(proto, "coveredLayers");
			}

			if (proto.HasInventory) {
				if (proto.WeightFraction.numerator == 0 && proto.WeightFraction.denominator == 0)
					MissingPropertyError(proto, "weightFraction");
			}
		}


		private static void MissingPropertyError(Prototype prototype, string propertyName) {
			Error($"Missing property '{propertyName}' in prototype '{prototype.Id}'.");
		}

		private static void Error(string message) {
			loadError = true;
			LogError(message);
		}

		private static void LogError(string message) {
			Terminal.WriteDirect("{red}([ERROR/Data]) ");
			Terminal.WriteLineDirect(message);
		}

		private static void LogWarning(string message) {
			Terminal.WriteDirect("{yellow}([WARN/Data]) ");
			Terminal.WriteLineDirect(message);
		}
	}
}