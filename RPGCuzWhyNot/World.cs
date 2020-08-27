using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGCuzWhyNot {
	public class World {
		private readonly List<Location> locations = new List<Location>();
		public ReadOnlyCollection<Location> Locations { get; }

		public World() {
			Locations = locations.AsReadOnly();
		}

		/// <summary>
		/// finds a location by its name. if not found it returns null
		/// </summary>
		/// <param name="callName">the name of the location</param>
		/// <returns></returns>
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

		public void MakePath(Location a, Location b) {
			
		}
	}
}