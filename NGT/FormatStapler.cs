//(c) 2018 Fancy Skeleton Games, Inc.

using NGT_Web.NGT;
using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Combines multiple Name Generators together using a format string (ie. use {0} to insert the result from the first source, {1} for the second, etc)")]
	public class FormatStapler : NameGenerator
	{
		public List<string> Sources = new();
		public string FormatString = "{0}";

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			var generators = GeneratorTracker.ResolveList(Sources);
			if (generators.Count == 0 || generators.Any(item => item == null))
			{
				return defaultVal;
			}

			string[] strings = new string[Sources.Count];

			for (int i = 0; i < Sources.Count; i++)
			{
				strings[i] = generators[i].Generate(defaultVal, random);
			}

			try
			{
				return string.Format(FormatString, strings);
			}
			catch
			{
				return defaultVal;
			}
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(FormatString)] = FormatString;
			data[nameof(Sources)] = Sources;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			FormatString = JsonSerializer.Deserialize<string>(data[nameof(FormatString)]);
			Sources = JsonSerializer.Deserialize<List<string>>(data[nameof(Sources)]);
		}
	}
}