using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;

namespace RPGCuzWhyNot.Things.Characters.NPCs
{
	public abstract class NPC : Character, IConverseable {
		public virtual void Converse(Character character, string response) {
			if (race is Humanoid humanoid) {
				Terminal.WriteLine($"No answer, it appears {humanoid.gender.referral} mute...");
			} else {
				Terminal.WriteLine($"No answer, it appears they're mute...");
			}
		}
	}
}

