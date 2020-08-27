using System;
using System.Collections.Generic;

namespace RPGCuzWhyNot {
	public class Player : Character {
		public void ReactToCommand(string[] args) {
			if (args.Length >= 1) {
				switch (args[0]) {
                    case "goto":
	                    if (args.Length >= 2) {
		                    Location location = Program.world.GetLocationByCallName(args[1]);
		                    if (location != null) {
			                    if (TryGoto(location)) {
				                    Console.WriteLine("success!");
			                    } else {
				                    Console.WriteLine("can't reach location from here");
			                    }
		                    } else {
			                    Console.WriteLine("Location not found, does it exist?");
                            }
	                    } else {
		                    Console.WriteLine("No location specified");
	                    }
						break;
                    case "equip":
	                    if (args.Length >= 2) {
							throw new System.NotImplementedException();

		                    //Todo: use args[1] to get the item
                            Item item = null;
                            if (item != null) {
	                            if (TryEquip(item)) {
		                            Console.WriteLine("success");
	                            }
                            } else {
								Console.WriteLine("Item not found, does it exist?");
                            }
                        } else {
		                    Console.WriteLine("No item specified");
                        }
						break;
					default:
						Console.WriteLine("Invalid command");
						break;
                }
            } else {
				Console.WriteLine("No command");
            }
		}

		private bool TryEquip(Item item) {
			throw new System.NotImplementedException();
        }

		private bool TryGoto(Location newLocation) {
			if (location.HasPathTo(newLocation)) {
				location = newLocation;
				return true;
			}

			return false;
		}
	}

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