using System;
using System.Text.Json.Serialization;

namespace RPGCuzWhyNot.Inventory.Item {
	[Serializable]
	public class Stats {
		[JsonPropertyName("speed")]
		public int Speed { get; set; }

		[JsonPropertyName("strength")]
		public int Strength { get; set; }

		[JsonPropertyName("accuracy")]
		public int Accuracy { get; set; }

		[JsonPropertyName("fortitude")]
		public int Fortitude { get; set; }

		public Stats() { }

		public Stats(int speed, int strength, int accuracy, int fortitude) {
			Speed = speed;
			Strength = strength;
			Accuracy = accuracy;
			Fortitude = fortitude;
		}
	}
}