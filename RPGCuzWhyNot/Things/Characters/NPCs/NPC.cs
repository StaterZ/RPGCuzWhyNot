using RPGCuzWhyNot.Systems;
using RPGCuzWhyNot.Things.Characters.Races;
using RPGCuzWhyNot.Things.Characters.Races.Humanoids;

namespace RPGCuzWhyNot.Things.Characters.NPCs {
	public abstract class NPC : Character, IConversable {
		public virtual void Converse(Character character, string response) {
			Terminal.WriteLine($"No answer, it appears {Referral.objectPronoun}'s mute...");
		}

		protected NPC(Race race) : base(race) {
		}
	}
}

