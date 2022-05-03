using System.Text.Json;

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

		protected override void SaveData(Dictionary<string, dynamic> data)
		{
			data[nameof(Min)] = Min;
			data[nameof(Max)] = Max;
		}

		protected override void LoadData(Dictionary<string, dynamic> data)
		{
			Min = JsonSerializer.Deserialize<int>(data[nameof(Min)]);
			Max = JsonSerializer.Deserialize<int>(data[nameof(Max)]);
		}
	}
}