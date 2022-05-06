//(c) 2018 Fancy Skeleton Games, Inc.

using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Picks a string at random from the supplied list.")]
	public class StringList : NameGenerator
	{
		public string[] Strings = null!;
		public bool SplitOnNewline = true;
		public bool SplitOnComma = true;
		public bool SplitOnSpace = false;

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Strings == null || Strings.Length <= 0)
			{
				return defaultVal;
			}

			return Strings.RandomElement(random);
		}

		public void SetStringFromText(string text)
		{
			List<char> splits = new();
			if (SplitOnNewline)
			{
				splits.Add('\n');
				splits.Add('\r');
			}
			if (SplitOnComma)
			{
				splits.Add(',');
			}
			if (SplitOnSpace)
			{
				splits.Add(' ');
			}

			Strings = text.Split(splits.ToArray());
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			if (Strings != null && Strings.Length > 0)
			{
				data[nameof(Strings)] = Strings;
			}

			data[nameof(SplitOnNewline)] = SplitOnNewline;
			data[nameof(SplitOnComma)] = SplitOnComma;
			data[nameof(SplitOnSpace)] = SplitOnSpace;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			if (data.Keys.Contains(nameof(Strings)))
			{
				Strings = JsonSerializer.Deserialize<string[]>(data[nameof(Strings)]);
			}

			SplitOnNewline = JsonSerializer.Deserialize<bool>(data[nameof(SplitOnNewline)]);
			SplitOnComma = JsonSerializer.Deserialize<bool>(data[nameof(SplitOnComma)]);
			SplitOnSpace = JsonSerializer.Deserialize<bool>(data[nameof(SplitOnSpace)]);
		}
	}
}