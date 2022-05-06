//(c) 2018 Fancy Skeleton Games, Inc.

using NGT_Web.NGT;
using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Picks a sub-generator at random to generate the final name from.")]
	public class RandomSelector : NameGenerator
	{
		public List<string> Sources = new();

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count == 0 || Sources.Any(item => item == null))
			{
				return defaultVal;
			}

			List<NameGenerator>? generators = GeneratorTracker.ResolveList(Sources);
			if (generators.Count == 0)
			{
				return defaultVal;
			}

			return generators.RandomElement(random).Generate(defaultVal, random);
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(Sources)] = Sources;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			Sources = JsonSerializer.Deserialize<List<string>>(data[nameof(Sources)]);
		}
	}
}
