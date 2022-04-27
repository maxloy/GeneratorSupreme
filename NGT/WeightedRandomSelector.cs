using System.Collections.Generic;
using System.Linq;

namespace NameGenToolkit
{
	[Description("Similar to the Random Selector, but each source module is assigned a weight so that some can be chosen more often than others. The weight values are relative to each other, not to an arbitrary total.")]
	public class WeightedRandomSelector : NameGenerator
	{
		[System.Serializable]
		public class WeightedSource
		{
			public NameGenerator Source = null!;
			public float Weight;
		}

		public List<WeightedSource> Sources = new List<WeightedSource>();

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count <= 0 || Sources.Any(item => item == null || item.Source == null))
				return defaultVal;

			float totalWeight = Sources.Sum(item => item.Weight);

			var rnd = random.NextDouble() * totalWeight;

			foreach (var source in Sources)
			{
				rnd -= source.Weight;
				if (rnd <= 0)
					return source.Source.Generate(defaultVal, random);
			}

			return defaultVal;
		}
	}

}