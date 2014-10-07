namespace HashDiff.Core
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	public class DifferenceCalculator
	{
		private string filename1;
		private string filename2;
		private string[] lines1;
		private string[] lines2;

		public bool IgnoreCase { get; set; }

		public bool Verbose { get; set; }

		public DifferenceCalculator(string file1, string file2)
		{
			filename1 = Path.GetFileName(file1);
			filename2 = Path.GetFileName(file2);
			lines1 = File.ReadAllLines(file1);
			lines2 = File.ReadAllLines(file2);
		}

		protected Dictionary<string, string> BuildMap(string[] lines)
		{
			var comparer = IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
			var map = new Dictionary<string, string>(comparer);

			foreach (var line in lines)
			{
				var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				var hash = parts[0];
				var filepath = parts[1];

				if (!map.ContainsKey(filepath))
					map.Add(filepath, hash);
			}

			return map;
		}

		protected Differences CalculateDifferences(string[] file1, string[] file2)
		{
			var map1 = BuildMap(file1);
			var map2 = BuildMap(file2);

			return CalculateDifferences(map1, map2);
		}

		protected Differences CalculateDifferences(Dictionary<string, string> map1, Dictionary<string, string> map2)
		{
			var diff = new Differences(filename1, filename2, IgnoreCase, Verbose);

			var allNames = new List<string>();
			allNames.AddRange(map1.Keys);
			allNames.AddRange(map2.Keys);

			foreach (var name in allNames.Distinct())
			{
				if (IgnoreFile(name))
				{
					continue;
				}
				else if (map1.ContainsKey(name) && (!map2.ContainsKey(name)) && !diff.Removed.ContainsKey(name))
				{
					diff.Removed.Add(name, map1[name]);
				}
				else if (map2.ContainsKey(name) && (!map1.ContainsKey(name)) && !diff.Added.ContainsKey(name))
				{
					diff.Added.Add(name, map2[name]);
				}
				else if (map1.ContainsKey(name) && (map2.ContainsKey(name)))
				{
					var entry1 = map1[name];
					var entry2 = map2[name];

					var match = string.Equals(entry1, entry2, StringComparison.OrdinalIgnoreCase);

					if (!match && !diff.Changed.ContainsKey(name))
					{
						diff.Changed.Add(name, new[] { entry1, entry2 });
					}
					else if (!diff.Unchanged.ContainsKey(name))
					{
						diff.Unchanged.Add(name, map2[name]);
					}
				}
			}

			return diff;
		}

		public Differences GetDifferences()
		{
			return CalculateDifferences(lines1, lines2);
		}

		protected bool IgnoreFile(string entryName)
		{
			if (string.IsNullOrWhiteSpace(entryName))
				return false;

			// TODO: [LK] Consider including logic to ignore certain files

			return false;
		}
	}
}