using System;
using System.Text.Json.Serialization;
using RPGCuzWhyNot.Utilities;

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

		public string Listing => $"SPD:{ConsoleUtils.FormatInt(Speed)}, STR:{ConsoleUtils.FormatInt(Strength)}, ACC:{ConsoleUtils.FormatInt(Accuracy)}, FRT:{ConsoleUtils.FormatInt(Fortitude)}";

		public Stats() { }

		public Stats(int speed, int strength, int accuracy, int fortitude) {
			Speed = speed;
			Strength = strength;
			Accuracy = accuracy;
			Fortitude = fortitude;
		}

		public static Stats operator +(Stats a, Stats b) {
			return new Stats(a.Speed + b.Speed, a.Strength + b.Strength, a.Accuracy + b.Accuracy, a.Fortitude + b.Fortitude);
		}

		public static Stats operator -(Stats a, Stats b) {
			return new Stats(a.Speed - b.Speed, a.Strength - b.Strength, a.Accuracy - b.Accuracy, a.Fortitude - b.Fortitude);
		}
	}
}