using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGCuzWhyNot {
	public class Referal {
		/// <summary>
		/// as in "did you know <subject> stole a knife?"
		/// in 1st person it would be "I"
		/// </summary>
		public readonly string subjectPronoun;

		/// <summary>
		/// as in "do you like <subject>?"
		/// in 1st person it would be "Me"
		/// </summary>
		public readonly string objectPronoun;


		/// <summary>
		/// as in "that's <subject> knife! you can't take that!"
		/// in 1st person it would be "My"
		/// </summary>
		public readonly string possesiveAdjective;

		/// <summary>
		/// as in "that knife is <subject>! you can't take that!"
		/// in 1st person it would be "Mine"
		/// </summary>
		public readonly string possesivePronoun;

		/// <summary>
		/// as in "control <subject>"
		/// in 1st person it would be "Myself"
		/// </summary>
		public readonly string reflexivePronoun;

		/// <param name="subjectPronoun">
		/// as in "did you know <subject> stole a knife?"
		/// in 1st person it would be "I"
		/// </param>
		/// <param name="objectPronoun">
		/// as in "do you like <subject>?"
		/// in 1st person it would be "Me"
		/// </param>
		/// <param name="possesiveAdjective">
		/// as in "that's <subject> knife! you can't take that!"
		/// in 1st person it would be "My"
		/// </param>
		/// <param name="possesivePronoun">
		/// as in "that knife is <subject>! you can't take that!"
		/// in 1st person it would be "Mine"
		/// </param>
		/// <param name="reflexivePronoun">
		/// as in "control <subject>"
		/// in 1st person it would be "Myself"
		/// </param>
		public Referal(string subjectPronoun, string objectPronoun, string possesiveAdjective, string possesivePronoun, string reflexivePronoun) {
			this.subjectPronoun = subjectPronoun;
			this.objectPronoun = objectPronoun;
			this.possesiveAdjective = possesiveAdjective;
			this.possesivePronoun = possesivePronoun;
			this.reflexivePronoun = reflexivePronoun;
		}
	}
}
