using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using RPGCuzWhyNot.Systems.Data.Prototypes;
using RPGCuzWhyNot.Things;
using RPGCuzWhyNot.Things.Characters.NPCs;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Data {
	public static class DataLoader {
		public enum ErrorLevel { Success, Warning, Error, }

		private static readonly string dataPath = "GameData" + Path.DirectorySeparatorChar;
		private static readonly string locationsPath = dataPath + "location";
		private static readonly string itemsPath = dataPath + "item";
		private static readonly string npcsPath = dataPath + "npc";
		private static readonly string lootTablesPath = dataPath + "loot";

		private static readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings {
			MissingMemberHandling = MissingMemberHandling.Error
		};

		private static readonly JsonSerializer serializer = JsonSerializer.CreateDefault(serializerSettings);
		private static readonly Random random = new Random();

		private static Dictionary<string, Prototype> prototypes;
		private static Dictionary<string, Location> locations;
		private static Dictionary<string, NPC> npcs;
		private static Dictionary<string, Type> npcTypeMap = new Dictionary<string, Type>();

		private static ErrorLevel loadErrorLevel;

		/// <summary>
		/// All of the loaded prototypes.
		/// </summary>
		public static ReadOnlyDictionary<string, Prototype> Prototypes { get; private set; }

		/// <summary>
		/// All of the loaded locations.
		/// </summary>
		public static ReadOnlyDictionary<string, Location> Locations { get; private set; }

		/// <summary>
		/// All of the loaded NPCs.
		/// </summary>
		public static ReadOnlyDictionary<string, NPC> NPCs { get; private set; }

		static DataLoader() {
			FindRegisteredNPCs();
		}

		/// <summary>
		/// Load all the game data files into the registry.
		/// </summary>
		public static ErrorLevel LoadGameData() {
			loadErrorLevel = ErrorLevel.Success;

			prototypes = new Dictionary<string, Prototype>();
			Prototypes = new ReadOnlyDictionary<string, Prototype>(prototypes);
			LoadPrototypesFromPath<ItemPrototype>(itemsPath);
			LoadPrototypesFromPath<LocationPrototype>(locationsPath);
			LoadPrototypesFromPath<NpcPrototype>(npcsPath);
			LoadPrototypesFromPath<LootTablePrototype>(lootTablesPath);
			ValidatePrototypes();

			ConstructLocations();
			InitializeLocations();
			ConstructNPCs();

			return loadErrorLevel;
		}

		/// <summary>
		/// Construct an item from the prototype registry.
		/// </summary>
		/// <param name="id">The id of the item</param>
		public static IItem CreateItem(string id) {
			if (prototypes.TryGetValue(id, out Prototype prototype) && prototype is ItemPrototype itemPrototype)
				return itemPrototype.Create();

			throw new DataLoaderException($"An item prototype with the id '{id}' was not found.");
		}

		/// <summary>
		/// Get an existing location from the registry.
		/// </summary>
		/// <param name="id">The id of the location.</param>
		public static Location GetLocation(string id) {
			if (!locations.TryGetValue(id, out Location location))
				throw new DataLoaderException($"A location with the id '{id} was not found.");

			return location;
		}

		/// <summary>
		/// Get an existing NPC from the registry.
		/// </summary>
		/// <param name="id">The id of the NPC.</param>
		public static NPC GetNPC(string id) {
			if (!npcs.TryGetValue(id, out NPC npc))
				throw new DataLoaderException($"An NPC with the id '{id}' was not found.");

			return npc;
		}

		private static void LoadPrototypesFromPath<TProto>(string path) where TProto : Prototype {
			if (!Directory.Exists(path)) {
				Warning($"Missing data directory \"{path}\".");
				return;
			}

			string[] dataFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);

			foreach (string filePath in dataFiles) {
				// Deserialize the prototype(s).
				List<TProto> prototypeList;
				try {
					string fileContent = File.ReadAllText(filePath);
					prototypeList = DeserializeOneOrMore<TProto>(fileContent);
				}
				catch (IOException e) {
					Error($"Failed to read file \"{filePath}\":\n{e.Message}");
					continue;
				}
				catch (JsonException e) {
					Error($"Failed to deserialize file \"{filePath}\":\n{e.Message}");
					continue;
				}

				// Add the new prototypes.
				foreach (TProto proto in prototypeList) {
					proto.DataFilePath = filePath;

					if (proto.Id == null) {
						MissingPropertyError(proto, "id");
						continue;
					}

					if (!prototypes.TryAdd(proto.Id, proto))
						Error($"Duplicate prototype definition of '{proto.Id}' in file \"{proto.DataFilePath}\".");
				}
			}
		}

		// Deserializes either an array or a single object from a string.
		private static List<T> DeserializeOneOrMore<T>(string json) {
			var reader = new JsonTextReader(new StringReader(json));
			var list = new List<T>();

			reader.Read();
			if (reader.TokenType == JsonToken.StartArray) {
				// Read an array of objects.
				reader.Read();
				while (reader.TokenType != JsonToken.EndArray) {
					T value = serializer.Deserialize<T>(reader);
					list.Add(value);
					reader.Read();
				}
			}
			else {
				// Read a single object.
				T value = serializer.Deserialize<T>(reader);
				list.Add(value);
			}

			if (reader.Read())
				throw new JsonException("Expected end of file after array/object.");

			return list;
		}

		private static NPC CreateNpc(NpcPrototype proto) {
			if (!npcTypeMap.TryGetValue(proto.Id, out Type type))
				return null;

			if (!locations.TryGetValue(proto.Location, out Location location))
				return null;

			NPC npc = (NPC)Activator.CreateInstance(type);
			if (npc == null) {
				Debug.Assert(false);
				return null;
			}

			npc.Name = proto.Name;
			npc.CallName = proto.CallName;
			npc.location = location;
			location.AddNPC(npc, proto.GlanceDescription, proto.ApproachDescription);

			return npc;
		}

		private static void ConstructLocations() {
			locations = new Dictionary<string, Location>();
			Locations = new ReadOnlyDictionary<string, Location>(locations);

			// Create the locations.
			foreach (LocationPrototype prototype in prototypes.Values.OfType<LocationPrototype>()) {
				Location location = prototype.Create();
				locations.Add(prototype.Id, location);
			}
		}

		private static void ConstructNPCs() {
			npcs = new Dictionary<string, NPC>();
			NPCs = new ReadOnlyDictionary<string, NPC>(npcs);

			// Create the NPCs.
			foreach (NpcPrototype proto in prototypes.Values.OfType<NpcPrototype>()) {
				NPC npc = CreateNpc(proto);
				if (npc != null) npcs.Add(proto.Id, npc);
			}
		}

		private static void InitializeLocations() {
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

				foreach (ThingWithChance item in locationPrototype.Items) {
					CreateItemInLocation(location, item);
				}
			}
		}

		private static void CreateItemInLocation(Location location, ThingWithChance itemDeclaration) {
			int itemCount = itemDeclaration.EvaluateChance(random);

			if (prototypes.TryGetValue(itemDeclaration.Id, out Prototype proto)) {
				switch (proto) {
					case ItemPrototype itemPrototype:
						for (int i = 0; i < itemCount; i++)
							location.items.MoveItem(itemPrototype.Create());
						return;

					case LootTablePrototype lootTable: {
						for (int i = 0; i < itemCount; i++) {
							string lootItemName = lootTable.Evaluate(random);
							var item = GetPrototype(lootItemName) as ItemPrototype;

							if (item == null) {
								Error($"Item '{lootItemName}' not found. Referenced by loot table '{itemDeclaration.Id}' used in location '{location.Prototype.Id}'.");
								continue;
							}

							location.items.MoveItem(item.Create());
						}

						return;
					}
				}
			}

			Error($"Item '{itemDeclaration.Id}' not found. Referenced by location '{location.Prototype.Id}'.");
		}

		private static void FindRegisteredNPCs() { // Find all registered NPCs (those marked with UniqueNpcAttribute).
			foreach (TypeInfo type in typeof(NPC).Assembly.DefinedTypes) {
				UniqueNpcAttribute attribute = type.GetCustomAttribute<UniqueNpcAttribute>();
				if (attribute != null) {
					if (!typeof(NPC).IsAssignableFrom(type)) {
						Error($"UniqueNpcAttribute used on a non NPC type '{type}'.");
					}
					else if (type.GetConstructor(Array.Empty<Type>()) == null) {
						Error($"Type marked with UniqueNpcAttribute '{type}' does not have a public parameterless constructor.");
					}
					else if (!npcTypeMap.TryAdd(attribute.Id, type)) {
						Error($"Duplicate UniqueNpcAttribute with id '{attribute.Id}' on type '{type}'.");
					}
				}
			}
		}

		private static Prototype GetPrototype(string id) {
			if (prototypes.TryGetValue(id, out Prototype proto))
				return proto;
			return null;
		}

		#region Validation

		private static void ValidatePrototypes() {
			foreach (Prototype proto in prototypes.Values) {
				switch (proto) {
					case LocationPrototype locationPrototype:
						ValidateLocationPrototype(locationPrototype);
						break;

					case ItemPrototype itemPrototype:
						ValidateItemPrototype(itemPrototype);
						break;

					case NpcPrototype npcPrototype:
						ValidateNpcPrototype(npcPrototype);
						break;

					case LootTablePrototype lootTablePrototype:
						ValidateLootTablePrototype(lootTablePrototype);
						break;
				}
			}
		}

		private static void ValidateLocationPrototype(LocationPrototype proto) {
			if (proto.Paths.Count == 0)
				Warning($"Location '{proto.Id}' has no paths.");

			if (proto.Paths.ContainsKey(proto.Id))
				Error($"Location '{proto.Id}' contains a path to itself.");
		}

		private static void ValidateItemPrototype(ItemPrototype proto) {
			if (proto.Inventory != null && proto.Wieldable == null)
				Error($"Item with inventory '{proto.Id}' must be wieldable, in file \"{proto.DataFilePath}\".");
		}

		private static void ValidateNpcPrototype(NpcPrototype proto) {
			if (!prototypes.TryGetValue(proto.Location, out Prototype locationPrototype) || !(locationPrototype is LocationPrototype))
				Error($"Unknown location '{proto.Location}' referenced by '{proto.Id}' in file \"{proto.DataFilePath}\".");

			if (!npcTypeMap.ContainsKey(proto.Id))
				Error($"Unknown NPC '{proto.Id}' in file \"{proto.DataFilePath}\".");
		}

		private static void ValidateLootTablePrototype(LootTablePrototype proto) {
			foreach (string id in proto.Items.Keys) {
				if (!(GetPrototype(id) is ItemPrototype))
					Error($"Item '{id}' not found. Referenced by loot table '{proto.Id}' in file \"{proto.DataFilePath}\".");
			}
		}

		#endregion


		private static void MissingPropertyError(Prototype prototype, string propertyName) {
			Error($"Missing property '{propertyName}' in prototype '{prototype.Id}', in file \"{prototype.DataFilePath}\".");
		}

		private static void Error(string message) {
			loadErrorLevel = ErrorLevel.Error;
			Terminal.WriteWithoutDelay("{red}([ERROR/Data]) ");
			Terminal.WriteLineRawWithoutDelay(message);
		}

		private static void Warning(string message) {
			if (loadErrorLevel < ErrorLevel.Warning)
				loadErrorLevel = ErrorLevel.Warning;
			Terminal.WriteWithoutDelay("{yellow}([WARN/Data]) ");
			Terminal.WriteLineRawWithoutDelay(message);
		}
	}
}