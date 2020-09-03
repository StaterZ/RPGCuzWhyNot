using RPGCuzWhyNot.Races.Humanoids;

namespace RPGCuzWhyNot.NPCs
{
	public abstract class NPC : Character, IConverseable {
		public virtual void Converse(Character character, string response) {
			if (race is Humanoid humanoid) {
				ConsoleUtils.SlowWriteLine($"No answer, it appears {humanoid.gender.referal} mute...");
			} else {
				ConsoleUtils.SlowWriteLine($"No answer, it appears they're mute...");
			}
		}
	}
}

