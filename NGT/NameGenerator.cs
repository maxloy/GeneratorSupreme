﻿//(c) 2018 Fancy Skeleton Games, Inc.

using NGT_Web.NGT;
using System.Collections;
using System.Text.Json;

namespace NameGenToolkit
{
	//Base NameGenerator class that drives the core functionality
	public abstract class NameGenerator
	{
		protected abstract string GenerateImpl(string defaultVal, System.Random random); //the core of the name generation logic lives here

		//set this to true if you want the system to track which things it has generated previously and attempt to not generate them again
		public bool Unique
		{
			get => _unique;
			set
			{
				_unique = value;
				createdNames.Clear();
			}
		}
		private bool _unique = false;

		static bool overrideUniqueSetting = false;

		static List<string> createdNames = new(); //internal list used to track which names have been generated previously
		const int maxTries = 1000; //maximum number of retries when attempting to regenerate names

		public string Name;
		private Guid _guid;
		public Guid GUID
		{
			get => _guid;
			set
			{
				GeneratorTracker.Deregister(this);
				_guid = value;
				GeneratorTracker.Register(this);
			}
		}

		public NameGenerator()
		{
			Name = "New " + GetType().Name;
			GUID = Guid.NewGuid();
		}

		~NameGenerator()
		{
			Console.WriteLine("Destroying " + Name);
			GeneratorTracker.Deregister(this);
		}

		/// <summary>
		/// Generates a string. If Unique is set to true, saves it into a list so that the value isn't generated again.
		/// </summary>
		/// <param name="defaultVal">If this is unable to generate a value, return this string instead</param>
		/// <param name="random">Option System.Random object to be used to determine random elements. Will fall back to Unity's Random class if left as null.</param>
		/// <returns>A string generated by the GenerateImpl function.</returns>
		public string Generate(string defaultVal, System.Random random)
		{
			if (!IsValid())
			{
				//Debug.LogError("Dependency loop detected", this);
				return "Dependency loop detected";
			}

			if (overrideUniqueSetting)
			{
				return GenerateImpl(defaultVal, random);
			}

			for (int i = 0; i < maxTries; i++)
			{
				string? val = GenerateImpl(defaultVal, random);
				if (createdNames.Contains(val) == false)
				{
					if (Unique)
					{
						createdNames.Add(val);
					}
					return val;
				}
			}

			return GenerateImpl(defaultVal, random);
		}

		public string Generate()
		{
			return Generate("DFLT", new System.Random(Random.Shared.Next(0, int.MaxValue)));
		}

		public string Generate(System.Random random)
		{
			return Generate("DFLT", random);
		}

		public string Generate(int seed)
		{
			return Generate("DFLT", new System.Random(seed));
		}

		public string Generate(string defaultVal)
		{
			return Generate(defaultVal, new System.Random());
		}

		public string Generate(string defaultVal, int seed)
		{
			return Generate(defaultVal, new System.Random(seed));
		}

		public virtual string Draw() { return ""; }

		/// <summary>
		/// Generates a string, but ignores the Unique flag. Used by the custom inspector to generate example values without messing with the in-game functionality.
		/// </summary>
		/// <param name="random">Option System.Random object to be used to determine random elements. Will fall back to Unity's Random class if left as null.</param>
		/// <returns>A string generated by the GenerateImpl function.</returns>
		public string Example()
		{
			if (!IsValid())
			{
				//Debug.LogError("Dependency loop detected", this);
				return "Dependency loop detected";
			}

			overrideUniqueSetting = true;

			string? returnVal = Generate("DFLT");

			overrideUniqueSetting = false;
			return returnVal;
		}

		//internal function used to check if there are any dependency loops
		bool IsValid()
		{
			return IsValid(new Stack<NameGenerator>());
		}

		bool IsValid(Stack<NameGenerator> callstack)
		{
			if (callstack.Contains(this))
			{
				return false;
			}

			callstack.Push(this);

			Stack<object> fieldInfoStack = new();

			foreach (System.Reflection.FieldInfo? field in GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
			{
				object? val = field.GetValue(this);
				if (val != null)
				{
					fieldInfoStack.Push(val);
				}
			}

			while (fieldInfoStack.Count > 0)
			{
				object? fieldObject = fieldInfoStack.Pop();

				if (fieldObject == null)
				{
					continue;
				}

				if (fieldObject is string stringField)
				{
					NameGenerator? other = GeneratorTracker.Find(stringField);
					if (other != null && other.IsValid(callstack) == false)
					{
						return false;
					}
					//if (genObject.IsValid(callstack) == false)
					//{
					//	return false;
					//}
				}
				else if (fieldObject is IEnumerable enumObject)
				{
					foreach (object? obj in enumObject)
					{
						fieldInfoStack.Push(obj);
					}
				}
				else
				{
					foreach (System.Reflection.FieldInfo? field in fieldObject.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
					{
						object? obj = field.GetValue(fieldObject);
						if (obj != null)
						{
							if (obj.Equals(fieldObject))
							{
								continue;
							}

							fieldInfoStack.Push(obj);
						}
					}
				}
			}

			callstack.Pop();

			return true;
		}

		/// <summary>
		/// Clears the list used to check if a name has been generated before.
		/// </summary>
		//[ContextMenu("Clear Generated Names")]
		public static void ClearGeneratedNames()
		{
			createdNames.Clear();
		}

		/// <summary>
		/// Sets the list used to check whether a name has been generated previously. Useful for restoring from save games.
		/// </summary>
		/// <param name="generatedNames"></param>
		public static void SetGeneratedNames(List<string> generatedNames)
		{
			createdNames = generatedNames;
		}

		/// <summary>
		/// Returns a list of all of the names previously generated when the Unique flag has been set to true. Useful for saving the game state for restoration later.
		/// </summary>
		public static List<string> GetAllGeneratedNames()
		{
			return createdNames;
		}

		//calls Example() 10 times. useful for testing output
		//[ContextMenu("Generate 10 Examples")]
		public void GenerateTen()
		{
			string s = "";
			for (int i = 0; i < 10; i++)
			{
				s += Example() + "\n";
			}
			Console.WriteLine(s);
		}

		public Dictionary<string, dynamic> Save()
		{
			Dictionary<string, dynamic> data = new();
			data[nameof(Name)] = Name;
			data[nameof(GUID)] = GUID.ToString();
			data["type"] = GetType().FullName!;

			SaveData(data);

			return data;
		}
		protected abstract void SaveData(Dictionary<string, dynamic> data);

		public void Load(Dictionary<string, dynamic> data)
		{
			Name = JsonSerializer.Deserialize<string>(data[nameof(Name)]);
			GUID = new Guid(JsonSerializer.Deserialize<string>(data[nameof(GUID)]));

			LoadData(data);
		}
		protected abstract void LoadData(Dictionary<string, dynamic> data);
	}

	//Add this attribute to NameGenerator subclasses to add a description to the inspector window
	[System.AttributeUsage(System.AttributeTargets.Class)]
	public class Description : System.Attribute
	{
		public string Desc;

		public Description(string desc)
		{
			Desc = desc;
		}
	}

}