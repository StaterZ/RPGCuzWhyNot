namespace RPGCuzWhyNot.Things.Characters.Races {
	public class Dragon : Species {
		public Gender gender;
		public override Referral Referral => gender.referral;

		public Dragon(Gender gender) {
			this.gender = gender;
		}
	}
}