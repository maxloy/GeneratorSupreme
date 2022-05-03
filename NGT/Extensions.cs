//(c) 2018 Fancy Skeleton Games, Inc.

namespace NameGenToolkit
{
	//some useful extensions for selecting random elements (something done a fair bit in this plugin)
	public static class Extensions
	{
		public static T RandomElement<T>(this IEnumerable<T> set, Random random)
		{
			return set.ElementAt(random.Next(0, set.Count()));
		}
	}
}
