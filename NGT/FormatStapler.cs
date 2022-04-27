//(c) 2018 Fancy Skeleton Games, Inc.

using System.Collections.Generic;
using System.Linq;

namespace NameGenToolkit
{
	//[CreateAssetMenu(menuName = MenuFolder + "/Format Stapler")]
	[Description("Combines multiple Name Generators together using a format string (ie. use {0} to insert the result from the first source, {1} for the second, etc)")]
	public class FormatStapler : NameGenerator
	{
		public List<NameGenerator> Sources = new List<NameGenerator>();
		public string FormatString = "{0}";

		protected override string GenerateImpl(string defaultVal, System.Random random)
		{
			if (Sources.Count == 0 || Sources.Any(item => item == null))
				return defaultVal;

			string[] strings = new string[Sources.Count];

			for (int i = 0; i < Sources.Count; i++)
			{
				strings[i] = Sources[i].Generate(defaultVal, random);
			}

			return string.Format(FormatString, strings);
		}
	}
}