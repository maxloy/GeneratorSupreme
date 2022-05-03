//(c) 2018 Fancy Skeleton Games, Inc.

using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Picks a string at random from the supplied list.")]
	public class StringList : NameGenerator
	{
		public string[] Strings = null!;

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Strings == null || Strings.Length <= 0)
			{
				return defaultVal;
			}

			return Strings.RandomElement(random);
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(Strings)] = Strings;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			Strings = JsonSerializer.Deserialize<string[]>(data[nameof(Strings)]);
		}
	}
}