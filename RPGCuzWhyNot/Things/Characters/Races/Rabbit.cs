namespace RPGCuzWhyNot.Things.Characters.Races {
	public class Rabbit : Species {
		public Gender gender;
		public override Referral Referral => gender.referral;

		public Rabbit(Gender gender) {
			this.gender = gender;
		}
	}
}