namespace HashDiff.Core
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	public class Generator
	{
		private string _rootpath;
		private HashAlgorithm _algorithm;
		private string[] _excludeDirectories;
		private string[] _excludeFiles;

		private string _emptyStringHash;
		public string EmptyStringHash
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(_emptyStringHash))
					return _emptyStringHash;

				var bytes = Encoding.Unicode.GetBytes(string.Empty);
				_emptyStringHash = ConvertHash(_algorithm.ComputeHash(bytes));

				return _emptyStringHash;
			}
		}

		public Generator(string path, HashAlgorithm algorithm, string[] excludeDirectories, string[] excludeFiles)
		{
			_rootpath = path;
			_algorithm = algorithm;
			_excludeDirectories = excludeDirectories;
			_excludeFiles = excludeFiles;
		}

		public string GenerateFileHashes()
		{
			var items = new List<string>();

			var directories = Directory.GetDirectories(_rootpath, "*", SearchOption.AllDirectories)
				.Where(x => !_excludeDirectories.Any(x.Contains));

			var files = Directory.GetFiles(_rootpath, "*.*", SearchOption.AllDirectories)
				.Where(x => !_excludeFiles.Any(x.Contains));

			items.AddRange(directories);
			items.AddRange(files);

			items.Sort();

			var list = new List<Md5sum>();

			foreach (var item in items)
			{
				var path = item.Replace(_rootpath, string.Empty).Replace("\\", "/").TrimStart('/');
				var attr = File.GetAttributes(item);
				if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
				{
					list.Add(new Md5sum(this.EmptyStringHash, path + "/"));
				}
				else
				{
					var bytes = File.ReadAllBytes(item);
					var hash = ConvertHash(_algorithm.ComputeHash(bytes));
					list.Add(new Md5sum(hash, path));
				}
			}

			return string.Join(Environment.NewLine, list);
		}

		internal static string ConvertHash(byte[] hash)
		{
			return BitConverter
				.ToString(hash)
				.Replace("-", string.Empty)
				.ToLower();
		}
	}

	//
	// TEMP POCO
	//
	internal class Md5sum
	{
		public Md5sum(string hash, string filepath)
		{
			this.Hash = hash;
			this.FilePath = filepath;
		}

		public string Hash { get; set; }

		public string FilePath { get; set; }

		public override string ToString()
		{
			return string.Format("{0}  {1}", this.Hash, this.FilePath);
		}
	}
}