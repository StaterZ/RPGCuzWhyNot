using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace RPGCuzWhyNot.Systems.Data.Prototypes {
	[Serializable, JsonObject(ItemRequired = Required.Always)]
	public class LootTablePrototype : Prototype {
		[JsonProperty("items")]
		public Dictionary<string, int> Items { get; set; }


		public string Evaluate(Random random) {
			int sumOfWeights = Items.Values.Sum();
			int value = random.Next(sumOfWeights);

			foreach ((string item, int weight) in Items) {
				if (value < weight)
					return item;
				value -= weight;
			}

			Debug.Fail("Should not get here.");
			return null;
		}
	}
}