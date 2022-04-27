
namespace NameGenToolkit
{
	[Description("Repeatedly calls a generator a variable number of times")]
	public class Repeater : NameGenerator
	{
		public NameGenerator Source = null!;
		public int Min = 1;
		public int Max = 3;
		public string Separator = " ";

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Source == null)
				return defaultVal;

			if (Min < 0 || Max < Min)
				return defaultVal;

			int iterations = random.Next(Min, Max + 1);

			string generatedString = "";

			for (int i = 0; i < iterations; i++)
			{
				generatedString += Source.Generate(defaultVal, random);

				if (i < iterations - 1)
					generatedString += Separator;
			}

			return generatedString;
		}
	}
}