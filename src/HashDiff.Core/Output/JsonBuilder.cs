namespace HashDiff.Core.Output
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	public class JsonBuilder : AbstractBuilder
	{
		public override void Build(StreamWriter writer, Differences diff)
		{
			Action<ICollection<string>, string> writeEntries = (keys, element) =>
			{
				var items = keys.Count > 0
					? string.Concat("\r\n    \"", string.Join("\",\r\n    \"", keys), "\"\r\n  ")
					: " ";

				writer.Write("  \"{0}\" : [{1}]", element, items);
			};

			writer.WriteLine("{");
			writer.WriteLine("  \"filename1\" : \"{0}\",", Path.GetFileName(diff.File1));
			writer.WriteLine("  \"filename2\" : \"{0}\",", Path.GetFileName(diff.File2));

			writeEntries(diff.Added.Keys, "added");
			writer.WriteLine(",");
			writeEntries(diff.Removed.Keys, "removed");
			writer.WriteLine(",");
			writeEntries(diff.Changed.Keys, "changed");

			writer.WriteLine("\r\n}");

			writer.Flush();
		}
	}
}