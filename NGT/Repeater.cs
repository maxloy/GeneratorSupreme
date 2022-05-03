
using NGT_Web.NGT;
using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Repeatedly calls a generator a variable number of times")]
	public class Repeater : NameGenerator
	{
		public string Source = "";
		public int Min = 1;
		public int Max = 3;
		public string Separator = " ";

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			var resolvedSource = GeneratorTracker.Find(Source)!;

			if (resolvedSource == null)
			{
				return defaultVal;
			}

			if (Min < 0 || Max < Min)
			{
				return defaultVal;
			}

			int iterations = random.Next(Min, Max + 1);

			string generatedString = "";

			for (int i = 0; i < iterations; i++)
			{
				generatedString += resolvedSource.Generate(defaultVal, random);

				if (i < iterations - 1)
				{
					generatedString += Separator;
				}
			}

			return generatedString;
		}

		protected override void SaveData (Dictionary<string, dynamic> data)
		{
			data[nameof(Min)] = Min;
			data[nameof(Max)] = Max;
			data[nameof(Separator)] = Separator;
			data[nameof(Source)] = Source;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			Min = JsonSerializer.Deserialize<int>(data[nameof(Min)]);
			Max = JsonSerializer.Deserialize<int>(data[nameof(Max)]);
			Separator = JsonSerializer.Deserialize<string>(data[nameof(Separator)]);
			Source = JsonSerializer.Deserialize<string>(data[nameof(Source)]);
		}
	}
}