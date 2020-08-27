using System;
using System.Collections.Generic;

namespace RPGCuzWhyNot {
	public class Location {
		public readonly string name;
		public readonly string callName;

        public Location(string callName, string name) {
	        this.callName = callName;
			this.name = name;
		}

		private readonly List<Location> paths = new List<Location>();
        public bool HasPathTo(Location location) {
			return paths.Contains(location);
		}

        public void AddPathTo(Location location) {
			if (paths.Contains(location)) throw new Exception("Locations are already connected");

			paths.Add(location);
			location.paths.Add(this);
        }
    }
}