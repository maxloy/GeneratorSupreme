//(c) 2018 Fancy Skeleton Games, Inc.

using System.Collections.Generic;
using System.Linq;

namespace NameGenToolkit
{
	[Description("Picks a sub-generator at random to generate the final name from.")]
	public class RandomSelector : NameGenerator
	{
		public List<NameGenerator> Sources = new List<NameGenerator>();

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count == 0 || Sources.Any(item => item == null))
				return defaultVal;

			return Sources.RandomElement(random).Generate(defaultVal, random);
		}
	}
}
