namespace RPGCuzWhyNot.Things.Characters {
	public class CharacterLocationData {
		public readonly Character character;
		public string glanceDescription;
		public string approachDescription;

		public CharacterLocationData(Character character, string glanceDescription, string approachDescription) {
			this.character = character;
			this.glanceDescription = glanceDescription;
			this.approachDescription = approachDescription;
		}
	}
}