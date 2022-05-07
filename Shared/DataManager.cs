using NGT_Web.NGT;
using System.Text.Json;

namespace NGT_Web.Shared
{
	public class DataManager
	{
		public string GeneratorCollectionName = "My Generator";
		public List<NameGenToolkit.NameGenerator> Data = new();

		public string OutputGeneratorID = "";
		
		public event EventHandler DataChanged = (obj, args) => { };

		public DataManager()
		{
			DataChanged += (a, b) => Save();
		}

		public List<Type> ImplementedTypes = new()
		{
			typeof(NameGenToolkit.ConsonantVowel),
			typeof(NameGenToolkit.RandomInteger),
			typeof(NameGenToolkit.Repeater),
			typeof(NameGenToolkit.StringList),
			typeof(NameGenToolkit.Stapler),
			typeof(NameGenToolkit.FormatStapler),
			typeof(NameGenToolkit.RandomSelector),
			typeof(NameGenToolkit.WeightedRandomSelector),
		};

		List<Type> SubgenTypes = new()
		{
			typeof(NameGenToolkit.FormatStapler),
			typeof(NameGenToolkit.Repeater),
			typeof(NameGenToolkit.RandomSelector),
			typeof(NameGenToolkit.WeightedRandomSelector),
			typeof(NameGenToolkit.Stapler),
		};

		Dictionary<Type, string> GeneratorIcons = new()
		{
			{ typeof(NameGenToolkit.FormatStapler), "formatstapler_icon" },
			{ typeof(NameGenToolkit.Repeater), "repeater_icon" },
			{ typeof(NameGenToolkit.RandomSelector), "randomselector_icon" },
			{ typeof(NameGenToolkit.WeightedRandomSelector), "randomselector_icon" },
			{ typeof(NameGenToolkit.Stapler), "stapler_icon" },
			{ typeof(NameGenToolkit.ConsonantVowel), "consonantvowel_icon" },
			{ typeof(NameGenToolkit.RandomInteger), "numbergen_icon" },
			{ typeof(NameGenToolkit.StringList), "stringlist_icon" },
		};

		public string GetIcon(Type genType, bool large = false)
		{
			if (GeneratorIcons.TryGetValue(genType, out string? icon))
			{
				return "Images/" + icon + (large ? "_2" : "") + ".png";
			}

			return "";
		}

		public bool UsesSubGenerators(Type type)
		{
			return SubgenTypes.Contains(type);
		}

		public bool DoesntUseSubGenerators(Type type)
		{
			return SubgenTypes.Contains(type) == false;
		}

		public void AddNewGenerator(Type generatorType)
		{
			if (ImplementedTypes.Contains(generatorType))
			{
				object? obj = Activator.CreateInstance(generatorType);
				if (obj != null)
				{
					AddGenerator((NameGenToolkit.NameGenerator)obj);
				}
			}
		}

		public void AddGenerator(NameGenToolkit.NameGenerator generator)
		{
			Data.Add(generator);
			DataChanged.Invoke(this, EventArgs.Empty);
		}

		public void RemoveGenerator(NameGenToolkit.NameGenerator generator)
		{
			Data.Remove(generator);
			GeneratorTracker.Deregister(generator);
			DataChanged.Invoke(this, EventArgs.Empty);
		}

		public void ClearAll()
		{
			foreach (NameGenToolkit.NameGenerator? gen in Data)
			{
				GeneratorTracker.Deregister(gen);
			}
			Data.Clear();
			DataChanged.Invoke(this, EventArgs.Empty);
		}

		public void FireDataChanged()
		{
			DataChanged.Invoke(this, EventArgs.Empty);
		}

		public void Load(string json)
		{
			Dictionary<string, dynamic> data = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(json)!;

			if (data == null)
			{
				//show error dialogue?

			}
			else
			{
				Data.Clear();

				GeneratorCollectionName = JsonSerializer.Deserialize<string>(data[nameof(GeneratorCollectionName)]);
				OutputGeneratorID = JsonSerializer.Deserialize<string>(data[nameof(OutputGeneratorID)]);
				Console.WriteLine(OutputGeneratorID);

				List<Dictionary<string, dynamic>> generatorData = JsonSerializer.Deserialize<List<Dictionary<string, dynamic>>>(data["data"]);
				foreach (Dictionary<string, dynamic> item in generatorData)
				{
					string typename = JsonSerializer.Deserialize<string>(item["type"]);
					Type? type = Type.GetType(typename);
					if (type != null)
					{
						if (Activator.CreateInstance(type) is NameGenToolkit.NameGenerator generator)
						{
							generator.Load(item);
							Data.Add(generator);
						}
						else
						{
							//error
						}
					}
					else
					{
						//error
					}
				}
			}

			FireDataChanged();
		}

		public string Save()
		{
			Dictionary<string, dynamic> data = new();

			data[nameof(GeneratorCollectionName)] = GeneratorCollectionName;
			data[nameof(OutputGeneratorID)] = OutputGeneratorID;

			List<Dictionary<string, dynamic>> generatorDataList = new();
			foreach (NameGenToolkit.NameGenerator? generator in Data)
			{
				generatorDataList.Add(generator.Save());
			}
			data["data"] = generatorDataList;

			string json = JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true });

			return json;
		}
	}
}
