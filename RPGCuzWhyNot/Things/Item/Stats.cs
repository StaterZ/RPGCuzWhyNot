using System;
using Newtonsoft.Json;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Things.Item {
	[Serializable]
	public class Stats {
		[JsonProperty("speed")]
		public int Speed { get; set; }

		[JsonProperty("strength")]
		public int Strength { get; set; }

		[JsonProperty("accuracy")]
		public int Accuracy { get; set; }

		[JsonProperty("fortitude")]
		public int Fortitude { get; set; }

		[JsonIgnore]
		public string Listing => $"SPD:{Utils.AddSignAndColor(Speed)}, STR:{Utils.AddSignAndColor(Strength)}, ACC:{Utils.AddSignAndColor(Accuracy)}, FRT:{Utils.AddSignAndColor(Fortitude)}";

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