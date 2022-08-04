using System;
using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.VirtualTerminal;
using RPGCuzWhyNot.Things.Characters;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot {
	public static class Program {
		public static Player player;

		private static int Main() {
			// Make sure virtual terminal control sequences works on windows.
			VT.EnableVirtualTerminalProcessing();

			try {
				int exitCode = Run();

				if (exitCode != 0) {
					Console.WriteLine($"Exited with bad code: {exitCode}");
					Utils.WaitForPlayer();
				}

				return exitCode;
			} catch (Exception e) {
				Console.WriteLine(e);
				Utils.WaitForPlayer();
				return -1;
			} finally {
				Terminal.ResetColor();
			}
		}

		private static int Run() {
			Terminal.IsCursorVisible = false; //default to not showing cursor


			//Load content
			if (!LoadContent()) {
				return 1;
			}

			//construct player
			player = DataLoader.CreatePlayer("player");

			//some basic event loop
			player.location.PrintEnterInformation();
			while (true) {
				Terminal.WriteLine();
				string commandText = Utils.Ask("|> ").ToLower();
				Terminal.WriteLine();
				player.Handle(commandText);
			}
		}

		private static bool LoadContent() { //Load content
			Terminal.WriteLineWithoutDelay("{fg:Yellow}(Loading Content...)");
			DataLoader.ErrorLevel errorLevel = DataLoader.LoadGameData();

			if (errorLevel == DataLoader.ErrorLevel.Error) {
				return false;
			}

			Terminal.WriteLineWithoutDelay("{fg:Green}(Done!)");

			if (errorLevel == DataLoader.ErrorLevel.Success) {
				Utils.Sleep(100);
				Terminal.Clear();
			}

			return true;
		}
	}
}