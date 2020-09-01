

namespace RPGCuzWhyNot {
	public interface IThing {
		string Name { get; }
		string Callname { get; }
	}

	public static class ThingExt {
		public static string ListingName(this IThing thing) {
			return $"{thing.Name} [{thing.Callname}]";
		}
	}
}

