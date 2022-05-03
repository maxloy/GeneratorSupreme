namespace NGT_Web.NGT
{
	public static class GeneratorTracker
	{
		private static Dictionary<string, NameGenToolkit.NameGenerator> guidToGenerator = new();

		public static void Register(NameGenToolkit.NameGenerator generator)
		{
			guidToGenerator[generator.GUID.ToString()] = generator;
		}

		public static void Deregister(NameGenToolkit.NameGenerator generator)
		{
			guidToGenerator.Remove(generator.GUID.ToString());
		}

		public static NameGenToolkit.NameGenerator? Find(string guid)
		{
			return guidToGenerator.GetValueOrDefault(guid);
		}

		public static List<NameGenToolkit.NameGenerator> ResolveList(List<string> guids)
		{
			List<NameGenToolkit.NameGenerator> list = new();
			foreach (string? guid in guids)
			{
				if (Find(guid) is NameGenToolkit.NameGenerator generator)
				{
					list.Add(generator);
				}
			}
			return list;
		}
	}
}
