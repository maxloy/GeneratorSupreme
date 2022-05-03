//(c) 2018 Fancy Skeleton Games, Inc.

namespace NameGenToolkit
{
	[Description("Picks a sub-generator at random to generate the final name from.")]
	public class RandomSelector : NameGenerator
	{
		public List<NameGenerator> Sources = new();

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count == 0 || Sources.Any(item => item == null))
			{
				return defaultVal;
			}

			return Sources.RandomElement(random).Generate(defaultVal, random);
		}

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			throw new NotImplementedException();
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			throw new NotImplementedException();
		}
	}
}
