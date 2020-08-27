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
            Location village = new Location("village", "The Village");
			Location dragonNest = new Location("nest", "The Elder Dragons Nest");

			world = new World();
            world.RegisterNewLocation(village);
			world.RegisterNewLocation(dragonNest);
			village.AddPathTo(dragonNest);

		    player = new Player {
			    name = "Bengt",
				location = village,
				race = new Human() {
					gender = Humanoid.Gender.Male
                }
		    };

		    Console.WriteLine("Enter commands to do shit!");

            //some basic event loop
            while (true) {
			    string commandText = ConsoleUtils.Ask("|>");
			    string[] segments = commandText.Split(' ');
			    player.ReactToCommand(segments);
            }
	    }
    }
}
