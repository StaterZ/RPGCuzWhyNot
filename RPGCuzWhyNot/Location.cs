using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPGCuzWhyNot {
	public class Location {
		public readonly string name;
		public readonly string callName;
		public readonly string description;
		public readonly string pathDescription;
		private readonly List<Location> paths = new List<Location>();
		public readonly ReadOnlyCollection<Location> Paths;

        public Location(string callName, string name, string description, string pathDescription) {
	        this.callName = callName;
			this.name = name;
			this.description = description;
			this.pathDescription = pathDescription;

			Paths = new ReadOnlyCollection<Location>(paths);
        }

        public bool HasPathTo(Location location) {
			return paths.Contains(location);
		}

        public void AddPathTo(Location location) {
			if (paths.Contains(location)) throw new Exception("Locations are already connected");

			paths.Add(location);
			location.paths.Add(this);
        }

        public override string ToString() {
	        return $"{name} [{callName}]";
        }

        public void PrintEnterInformation() {
	        string title = $"----- [ {name} ] -----";
	        Console.WriteLine(title);
	        PrintInformation();
	        Console.WriteLine(new string('-', title.Length));
        }

        public void PrintInformation() {
	        Console.WriteLine(description);
	        foreach (Location location in paths) {
		        Console.WriteLine(location.pathDescription + " [" + location.callName + "]");
	        }
        }
	}
}