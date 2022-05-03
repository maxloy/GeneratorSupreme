//(c) 2018 Fancy Skeleton Games, Inc.

using System.Text.Json;

namespace NameGenToolkit
{
	[Description("Generates a string in Consonant-Vowel-Consonant-Vowel-Consonant-etc format")]
	public class ConsonantVowel : NameGenerator
	{
		public int MinLength = 5;
		public int MaxLength = 5;

		static List<char> consonant = new() { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'x', 'z' };
		static List<char> vowel = new() { 'a', 'e', 'i', 'o', 'u', 'y' };

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (MinLength <= 0 || MaxLength <= 0)
			{
				return defaultVal;
			}

			int length = random.Next(Math.Min(MinLength, MaxLength), Math.Max(MinLength, MaxLength));
			char[]? array = new char[length];
			for (int i = 0; i < length; i++)
			{
				if (i % 2 == 0)
				{
					array[i] = consonant.RandomElement(random);
				}
				else
				{
					array[i] = vowel.RandomElement(random);
				}
			}

			array[0] = char.ToUpper(array[0]);

			return new string(array);
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(MinLength)] = MinLength;
			data[nameof(MaxLength)] = MaxLength;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			MinLength = JsonSerializer.Deserialize<int>(data[nameof(MinLength)]);
			MaxLength = JsonSerializer.Deserialize<int>(data[nameof(MaxLength)]);
		}
	}
}
