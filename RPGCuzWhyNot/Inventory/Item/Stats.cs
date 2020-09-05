namespace RPGCuzWhyNot.Inventory.Item {
	public class Stats {
		public int Speed { get; set; }
		public int Strength { get; set; }
		public int Accuracy { get; set; }
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