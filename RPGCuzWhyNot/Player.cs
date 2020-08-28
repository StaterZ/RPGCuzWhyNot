using System;
using System.Collections.Generic;

namespace RPGCuzWhyNot {
	public class Player : Character {
		public readonly List<Item> inventory = new List<Item>();

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
			if (args.Length == 0) return;

			switch (args[0].ToLower()) {
				// Go to another location.
				case "go":
				case "goto":
				case "enter":
					if (args.Length >= 2) {
						Location newLocation = Program.world.GetLocationByCallName(args[1]);
						if (newLocation != null && TryGoto(newLocation)) {
							location.PrintEnterInformation();
						} else {
							Console.WriteLine("I don't know where that is.");
						}
					} else {
						Console.WriteLine("Where to?");
					}

					break;

				// Show information about the current location.
				case "where":
					Console.WriteLine($"You are in: {location}");
					location.PrintInformation();
					break;

				// List all locations accessible from the current one.
				case "ls":
				case "list":
				case "locations":
					Console.WriteLine("Locations:");
					foreach (Location loc in location.Paths) {
						Console.Write("  ");
						Console.WriteLine(loc);
					}

					break;

				case "equip":
					if (args.Length >= 2) {
						throw new NotImplementedException();

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

				// Take an item from the current location.
				case "take":
				case "pickup":
				case "grab":
					if (args.Length < 2) {
						Console.WriteLine("Take what?");
						break;
					}

					if (!TryPickup(args[1])) {
						Console.WriteLine("Can't see that here.");
					}

					break;

				// List the items in the inventory.
				case "inventory":
					Console.WriteLine("Inventory:");
					foreach (Item item in inventory) {
						Console.Write("  ");
						Console.WriteLine(item);
					}

					break;

				// List the items in the current location.
				case "items":
					if (location.Items.Count != 0) {
						Console.WriteLine("You look around and see:");
						foreach (Item item in location.Items) {
							Console.Write("  ");
							Console.WriteLine(item);
						}
					}
					else {
						Console.WriteLine("You look around but can't find anything of use.");
					}

					break;

				default:
					Console.WriteLine("I don't understand.");
					break;
			}

		}

		private bool TryEquip(Item item) {
			throw new NotImplementedException();
		}

		private bool TryGoto(Location newLocation) {
			if (location.HasPathTo(newLocation)) {
				location = newLocation;
				return true;
			}

			return false;
		}

		private bool TryPickup(string callName) {
			Item item = location.GetItemByCallName(callName);

			if (item != null) {
				location.RemoveItem(item);
				inventory.Add(item);
				Console.WriteLine($"You picked up: {item}.");
				return true;
			}

			return false;
		}
	}
}