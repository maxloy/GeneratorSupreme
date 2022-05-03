//(c) 2018 Fancy Skeleton Games, Inc.

using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Joins two or more generators together using a supplied separator string")]
	public class Stapler : NameGenerator
	{
		public List<NameGenerator> Sources = new();
		public string Separator = " ";

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count == 0 || Sources.Any(item => item == null))
			{
				return defaultVal;
			}

			string? finalName = Sources[0].Generate(defaultVal, random);

			for (int i = 1; i < Sources.Count; i++)
			{
				finalName += Separator + Sources[i].Generate(defaultVal, random);
			}

			return finalName;
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(Separator)] = Separator;
			throw new NotImplementedException();
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			Separator = JsonSerializer.Deserialize<string>(data[nameof(Separator)]);
			throw new NotImplementedException();
		}
	}
}