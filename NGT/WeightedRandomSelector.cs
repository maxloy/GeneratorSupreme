using NGT_Web.NGT;
using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Similar to the Random Selector, but each source module is assigned a weight so that some can be chosen more often than others. The weight values are relative to each other, not to an arbitrary total.")]
	public class WeightedRandomSelector : NameGenerator
	{

		public List<string> Sources = new();
		public List<float> Weights = new();

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count == 0)
			{
				return defaultVal;
			}

			List<NameGenerator>? generators = GeneratorTracker.ResolveList(Sources);
			if (generators.Count == 0)
			{
				return defaultVal;
			}

			double totalWeight = Weights.Sum();

			try
			{
				double rnd = random.NextDouble() * totalWeight;

				for (int i = 0; i < generators.Count; i++)
				{
					NameGenerator? generator = generators[i];
					float weight = Weights[i];
					rnd -= weight;
					if(rnd < 0)
					{
						return generator.Generate(defaultVal, random);
					}
				}
			}
			catch
			{
				return defaultVal;
			}

			return defaultVal;
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(Sources)] = Sources;
			data[nameof(Weights)] = Weights;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			Sources = JsonSerializer.Deserialize<List<string>>(data[nameof(Sources)]);
			Weights = JsonSerializer.Deserialize<List<float>>(data[nameof(Weights)]);
		}
	}

}