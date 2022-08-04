using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Things.Characters.Races;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;
using RPGCuzWhyNot.Things.Item;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable]
	public class PlayerPrototype : Prototype {
		[JsonProperty("spawnLocation", Required = Required.Always)]
		public string SpawnLocation { get; set; }

		[JsonProperty("stats", Required = Required.Always)]
		public Stats Stats { get; set; }

		[JsonProperty("inventory")]
		public string[] Inventory { get; set; }

		[JsonProperty("wielding")]
		public string[] Wielding { get; set; }

		[JsonProperty("wearing")]
		public string[] Wearing { get; set; }

		public Player Create() {
			Species species = new Human(Gender.Male); //TODO: fix hard-coded species
			Player player = new Player(species) {
				Name = Name,
				location = DataLoader.GetLocation(SpawnLocation),
				stats = Stats
			};

			if (Inventory != null) {
				foreach (string item in Inventory) {
					player.Inventory.MoveItem(DataLoader.CreateItem(item));
				}
			}

			if (Wielding != null) {
				foreach (string item in Wielding) {
					player.Wielding.MoveItem((IWieldable)DataLoader.CreateItem(item));
				}
			}

			if (Wearing != null) {
				foreach (string item in Wearing) {
					player.Wearing.MoveItem((IWearable)DataLoader.CreateItem(item));
				}
			}

			return player;
		}
	}
}
