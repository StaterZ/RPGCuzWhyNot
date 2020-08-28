using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGCuzWhyNot {
	public class World {
		private readonly List<Location> locations = new List<Location>();
		public readonly ReadOnlyCollection<Location> Locations;

		public World() {
			Locations = locations.AsReadOnly();
		}

		public Location GetLocationByCallName(string callName) {
			foreach (Location location in locations) {
				if (location.callName == callName) {
					return location;
				}
			}

			return null;
		}

		public void RegisterNewLocation(Location location) {
			locations.Add(location);
		}
	}
}