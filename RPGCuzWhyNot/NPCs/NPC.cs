using RPGCuzWhyNot.Races;
using RPGCuzWhyNot.Races.Humanoids;

namespace RPGCuzWhyNot.NPCs
{
	public abstract class NPC : Character, IConverseable {
		public virtual void Converse(Character character, string response) {
			if (race is Humanoid humanoid) {
				Terminal.WriteLine($"No answer, it appears {humanoid.gender.referal} mute...");
			} else {
				Terminal.WriteLine($"No answer, it appears they're mute...");
			}
		}

		protected NPC(Race race) : base(race) {
		}
	}
}

