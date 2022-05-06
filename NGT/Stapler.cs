//(c) 2018 Fancy Skeleton Games, Inc.

using NGT_Web.NGT;
using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Joins two or more generators together using a supplied separator string")]
	public class Stapler : NameGenerator
	{
		public List<string> Sources = new();
		public string Separator = " ";

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count == 0)
			{
				return defaultVal;
			}
			foreach(var source in Sources)
			{
				Console.WriteLine(source);
			}

			List<NameGenerator>? generators = GeneratorTracker.ResolveList(Sources);
			if (generators.Count == 0)
			{
				return defaultVal;
			}

			string? finalName = generators[0].Generate(defaultVal, random);

			for (int i = 1; i < Sources.Count; i++)
			{
				finalName += Separator + generators[i].Generate(defaultVal, random);
			}

			return finalName;
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(Separator)] = Separator;
			data[nameof(Sources)] = Sources;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			Separator = JsonSerializer.Deserialize<string>(data[nameof(Separator)]);
			Sources = JsonSerializer.Deserialize<List<string>>(data[nameof(Sources)]);
		}
	}
}