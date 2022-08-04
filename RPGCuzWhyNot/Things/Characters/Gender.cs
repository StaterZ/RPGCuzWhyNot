namespace RPGCuzWhyNot.Things.Characters
{
	public class Gender {
		public static readonly Gender Male = new Gender(new Referral("he", "him", "his", "his", "himself"));
		public static readonly Gender Female = new Gender(new Referral("she", "her", "her", "hers", "herself"));
		public static readonly Gender Neutral = new Gender(new Referral("they", "them", "their", "theirs", "themselves"));

		public readonly Referral referral;

		public Gender(Referral referral) {
			this.referral = referral;
		}
	}
}