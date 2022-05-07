//(c) 2018 Fancy Skeleton Games, Inc.

using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Picks a string at random from the supplied list.")]
	public class StringList : NameGenerator
	{
		public string RawValue = "";
		public bool SplitOnNewline = true;
		public bool SplitOnComma = true;
		public bool SplitOnSpace = true;

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (string.IsNullOrEmpty(RawValue))
			{
				return defaultVal;
			}

			return SplitText(RawValue).RandomElement(random);
		}

		private string[] SplitText(string text)
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

			return text.Split(splits.ToArray());
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			if (!string.IsNullOrEmpty(RawValue))
			{
				data[nameof(RawValue)] = RawValue;
			}

			data[nameof(SplitOnNewline)] = SplitOnNewline;
			data[nameof(SplitOnComma)] = SplitOnComma;
			data[nameof(SplitOnSpace)] = SplitOnSpace;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			if (data.ContainsKey(nameof(RawValue)))
{
				RawValue = JsonSerializer.Deserialize(data[nameof(RawValue)]);
			}

			SplitOnNewline = JsonSerializer.Deserialize<bool>(data[nameof(SplitOnNewline)]);
			SplitOnComma = JsonSerializer.Deserialize<bool>(data[nameof(SplitOnComma)]);
			SplitOnSpace = JsonSerializer.Deserialize<bool>(data[nameof(SplitOnSpace)]);
		}
	}
}