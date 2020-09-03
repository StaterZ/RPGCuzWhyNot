using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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
		private static Dictionary<string, Location> locations = new Dictionary<string, Location>();

		public static ReadOnlyDictionary<string, Location> Locations { get; private set; } =
			new ReadOnlyDictionary<string, Location>(locations);

		public static void LoadGameData() {
			locationPrototypes = LoadPrototypesFromPath<LocationPrototype>(LocationsPath)
				.ToDictionary(proto => proto.CallName);
			itemPrototypes = LoadPrototypesFromPath<ItemPrototype>(ItemsPath)
				.ToDictionary(proto => proto.CallName);

			locations = locationPrototypes.Values
				.Select(proto => proto.Instantiate())
				.ToDictionary(location => location.CallName);

			Locations = new ReadOnlyDictionary<string, Location>(locations);

			SetupLocations();
		}

		private static List<TProto> LoadPrototypesFromPath<TProto>(string path) where TProto : Prototype {
			string[] dataFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
			List<TProto> prototypes = new List<TProto>(dataFiles.Length);

			foreach (string filePath in dataFiles) {
				string fileContent = File.ReadAllText(filePath);
				TProto prototype = JsonSerializer.Deserialize<TProto>(fileContent, serializerOptions);
				prototype.CallName = Path.GetFileNameWithoutExtension(filePath);
				prototypes.Add(prototype);
			}

			return prototypes;
		}

		private static void SetupLocations() {
			foreach ((string name, Location location) in locations) {
				LocationPrototype locationPrototype = locationPrototypes[name];

				// Add all the paths to the location.
				foreach (string path in locationPrototype.Paths) {
					if (!locations.TryGetValue(path, out Location destination))
						throw new Exception($"Referenced location '{path}' not found");

					location.AddPathTo(destination);
				}

				// Create the items in the location.
				foreach (string itemName in locationPrototype.Items) {
					if (!itemPrototypes.TryGetValue(itemName, out ItemPrototype item))
						throw new Exception($"Referenced item '{itemName}' not found");

					location.items.MoveItem(item.Instantiate());
				}
			}
		}
	}
}