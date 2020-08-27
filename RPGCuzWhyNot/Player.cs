using System;

namespace RPGCuzWhyNot {
	public class Player : Character {
		public override void TakeDamage(int damage, Character source) {
			damage = Math.Min(health, damage);
			health -= damage;

			Console.WriteLine($"{source.name} hit you for {damage} damage.");

			if (health <= 0) {
				Console.WriteLine($"{source.name} killed you.");
				// TODO: Die
			}
		}
		
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
}