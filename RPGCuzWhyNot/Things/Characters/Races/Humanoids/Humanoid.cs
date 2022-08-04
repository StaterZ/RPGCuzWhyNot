namespace RPGCuzWhyNot.Things.Characters.Races.Humanoids {
	public abstract class Humanoid : Species {
		public Gender gender;
		public override Referral Referral => gender.referral;

		protected Humanoid(Gender gender) {
			this.gender = gender;
		}
	}
}