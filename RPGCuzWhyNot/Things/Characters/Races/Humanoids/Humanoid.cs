namespace RPGCuzWhyNot.Things.Characters.Races.Humanoids {
	public abstract class Humanoid : Race {
		public Gender gender;

		public class Gender {
			public static readonly Gender Male = new Gender(new Referal("he", "him", "his", "his", "himself"));
			public static readonly Gender Female = new Gender(new Referal("she", "her", "her", "hers", "herself"));
			public static readonly Gender Neutral = new Gender(new Referal("they", "them", "their", "theirs", "themselves"));

			public readonly Referal referal;

			public Gender(Referal referal) {
				this.referal = referal;
			}
		}
	}
}