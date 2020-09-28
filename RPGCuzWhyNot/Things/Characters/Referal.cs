namespace RPGCuzWhyNot.Things.Characters {
	public class Referal {
		/// <summary>
		/// as in "did you know [subject] stole a knife?"
		/// in 1st person it would be "I"
		/// </summary>
		public readonly string subjectPronoun;

		/// <summary>
		/// as in "do you like [subject]?"
		/// in 1st person it would be "Me"
		/// </summary>
		public readonly string objectPronoun;


		/// <summary>
		/// as in "that's [subject] knife! you can't take that!"
		/// in 1st person it would be "My"
		/// </summary>
		public readonly string possessiveAdjective;

		/// <summary>
		/// as in "that knife is [subject]! you can't take that!"
		/// in 1st person it would be "Mine"
		/// </summary>
		public readonly string possessivePronoun;

		/// <summary>
		/// as in "control [subject]"
		/// in 1st person it would be "Myself"
		/// </summary>
		public readonly string reflexivePronoun;

		/// <param name="subjectPronoun">
		/// as in "did you know [subject] stole a knife?"
		/// in 1st person it would be "I"
		/// </param>
		/// <param name="objectPronoun">
		/// as in "do you like [subject]?"
		/// in 1st person it would be "Me"
		/// </param>
		/// <param name="possessiveAdjective">
		/// as in "that's [subject] knife! you can't take that!"
		/// in 1st person it would be "My"
		/// </param>
		/// <param name="possessivePronoun">
		/// as in "that knife is [subject]! you can't take that!"
		/// in 1st person it would be "Mine"
		/// </param>
		/// <param name="reflexivePronoun">
		/// as in "control [subject]"
		/// in 1st person it would be "Myself"
		/// </param>
		public Referal(string subjectPronoun, string objectPronoun, string possessiveAdjective, string possessivePronoun, string reflexivePronoun) {
			this.subjectPronoun = subjectPronoun;
			this.objectPronoun = objectPronoun;
			this.possessiveAdjective = possessiveAdjective;
			this.possessivePronoun = possessivePronoun;
			this.reflexivePronoun = reflexivePronoun;
		}
	}
}
