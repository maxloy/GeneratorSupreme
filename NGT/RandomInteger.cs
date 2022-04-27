namespace NameGenToolkit
{
	[Description("Generates a random number between the specified Min and Max values (inclusive)")]
	public class RandomInteger : NameGenerator
	{
		public int Min = 0;
		public int Max = 99;

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			return random.Next(Min, Max + 1).ToString();
		}
	}
}