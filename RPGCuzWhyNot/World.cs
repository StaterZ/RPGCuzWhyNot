using System.Collections.Generic;

namespace RPGCuzWhyNot {
	public class World {
		private readonly List<Location> locations = new List<Location>();

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