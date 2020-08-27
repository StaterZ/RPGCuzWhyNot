using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCuzWhyNot {
    public static class Program {
		public static Player player;
		public static World world;

		private static void Main(string[] args) {
			//construct world
            Location village = new Location(
	            "village", 
	            "The Village", 
	            "A small village stands before you.", 
	            "The smoke of a little village can be seen in the distance."
	        );
			Location dragonNest = new Location(
				"nest", 
				"The Elder Dragons Nest", 
				"A large lair sprawls out around you. The deep ominous sound of the dragon breathing can be heard further in.", 
				"In the horizon a dragons nest can be seen"
			);

			world = new World();
            world.RegisterNewLocation(village);
			world.RegisterNewLocation(dragonNest);
			village.AddPathTo(dragonNest);

			//construct player
            player = new Player {
			    name = "Bengt",
				location = village,
				race = new Human {
					gender = Humanoid.Gender.Male
                }
		    };

            //some basic event loop
			player.location.PrintEnterInformation();
            while (true) {
	            Console.WriteLine();
                string commandText = ConsoleUtils.Ask("|>");
			    string[] segments = commandText.Split(' ');
			    player.ReactToCommand(segments);
            }
	    }
    }
}
