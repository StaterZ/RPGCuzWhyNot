using System;

namespace RPGCuzWhyNot.Systems.Data {
	/// <summary>
	/// Placed on NPC classes to make them visible to, and used by, the data loader.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class UniqueNpcAttribute : Attribute {
		/// <summary>
		/// The id of the NPC.
		/// </summary>
		public string Id { get; }

		public UniqueNpcAttribute(string id) {
			Id = id;
		}
	}
}