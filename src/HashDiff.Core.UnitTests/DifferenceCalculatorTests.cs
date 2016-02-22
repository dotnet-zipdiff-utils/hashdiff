namespace HashDiff.Core.UnitTests
{
	using System.IO;
	using System.Linq;
	using HashDiff.Core.Output;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class DifferenceCalculatorTests
	{
		const string TestHashes_A1 = "test-hashes-a1.md5";
		const string TestHashes_A1_Changed = "test-hashes-a1-changed.md5";
		const string TestHashes_A2 = "test-hashes-a2.md5";
		const string TestHashes_B1 = "test-hashes-b1.md5";

		[TestInitialize]
		public void Init()
		{
			this.CreateTestHashes(TestHashes_A1);
			this.CreateTestHashes(TestHashes_A2);
			this.CreateTestHashesContentsChanged(TestHashes_A1_Changed);
			this.CreateTestHashesContentsDifferent(TestHashes_B1);
		}

		[TestCleanup]
		public void Cleanup()
		{
			var files = new[] { TestHashes_A1, TestHashes_A1_Changed, TestHashes_A2, TestHashes_B1 };
			foreach (var file in files.Where(File.Exists))
				File.Delete(file);
		}

		[TestMethod]
		public void CalculateDifferences_SameFile()
		{
			var calc = new DifferenceCalculator(TestHashes_A1, TestHashes_A1);
			var diff = calc.GetDifferences();

			Assert.IsFalse(diff.HasDifferences());
			Assert.IsTrue(diff.Added.Count == 0);
			Assert.IsTrue(diff.Removed.Count == 0);
			Assert.IsTrue(diff.Changed.Count == 0);

			this.ExerciseOutputBuilders(diff);
		}

		[TestMethod]
		public void CalculateDifferences_SameEntries()
		{
			var calc = new DifferenceCalculator(TestHashes_A1, TestHashes_A2);
			var diff = calc.GetDifferences();

			Assert.IsFalse(diff.HasDifferences());
			Assert.IsTrue(diff.Added.Count == 0);
			Assert.IsTrue(diff.Removed.Count == 0);
			Assert.IsTrue(diff.Changed.Count == 0);

			this.ExerciseOutputBuilders(diff);
		}

		[TestMethod]
		public void CalculateDifferences_DifferentEntries()
		{
			var calc = new DifferenceCalculator(TestHashes_A1, TestHashes_B1);
			var diff = calc.GetDifferences();

			Assert.IsTrue(diff.HasDifferences());
			Assert.IsTrue(diff.Added.ContainsKey("GGGGGGGG.txt"));
			Assert.IsTrue(diff.Removed.ContainsKey("WWWWWWWW.txt"));
			Assert.IsTrue(diff.Changed.Count == 0);

			this.ExerciseOutputBuilders(diff);
		}

		[TestMethod]
		public void CalculateDifferences_SameEntriesDifferentContent()
		{
			var calc = new DifferenceCalculator(TestHashes_A1, TestHashes_A1_Changed);
			var diff = calc.GetDifferences();

			Assert.IsTrue(diff.HasDifferences());
			Assert.IsTrue(diff.Added.Count == 0);
			Assert.IsTrue(diff.Removed.Count == 0);
			Assert.IsTrue(diff.Changed.ContainsKey("AAAAAAAA.txt"));

			this.ExerciseOutputBuilders(diff);
		}

		// TODO: Write tests to compare zips with same entries, but case-sensitive names

		private void CreateTestHashes(string filename)
		{
			var contents = new[]
			{
				"a7bec96155434df4b21fcd08c527337c  AAAAAAAA.txt",
				"2d033a8fe6e5cc9af295dcd99bf9c5e8  BBBBBBBB.txt",
				"9448bd6e52fa2a735f872a78cebe21b6  CCCCCCCC.txt",
				"f09d69188c915ea984f6eb8887f6a2d9  DDDDDDDD.txt",
				"792f1ac71da2ce43086f5fb02765a90e  EEEEEEEE.txt",
				"706d470269464276b0bb8a26c3417b1b  FFFFFFFF.txt",
				"a4fae6e1ca60886591eac80fd71e4ca1  WWWWWWWW.txt",
				"da60f33aa1791392ddcde69d407a1e81  XXXXXXXX.txt",
				"39b0fb779fdfd5b64a9b57733a2b32f2  YYYYYYYY.txt",
				"98dcc0fe09e1d88e8168eaba815806a8  ZZZZZZZZ.txt"
			};

			File.WriteAllLines(filename, contents);
		}

		private void CreateTestHashesContentsChanged(string filename)
		{
			var contents = new[]
			{
				"98c7ffa75e552f54ad459a8d1da585f3  AAAAAAAA.txt",
				"2d033a8fe6e5cc9af295dcd99bf9c5e8  BBBBBBBB.txt",
				"9448bd6e52fa2a735f872a78cebe21b6  CCCCCCCC.txt",
				"f09d69188c915ea984f6eb8887f6a2d9  DDDDDDDD.txt",
				"792f1ac71da2ce43086f5fb02765a90e  EEEEEEEE.txt",
				"706d470269464276b0bb8a26c3417b1b  FFFFFFFF.txt",
				"a4fae6e1ca60886591eac80fd71e4ca1  WWWWWWWW.txt",
				"da60f33aa1791392ddcde69d407a1e81  XXXXXXXX.txt",
				"39b0fb779fdfd5b64a9b57733a2b32f2  YYYYYYYY.txt",
				"98dcc0fe09e1d88e8168eaba815806a8  ZZZZZZZZ.txt"
			};

			File.WriteAllLines(filename, contents);
		}

		private void CreateTestHashesContentsDifferent(string filename)
		{
			var contents = new[]
			{
				"a7bec96155434df4b21fcd08c527337c  AAAAAAAA.txt",
				"2d033a8fe6e5cc9af295dcd99bf9c5e8  BBBBBBBB.txt",
				"9448bd6e52fa2a735f872a78cebe21b6  CCCCCCCC.txt",
				"f09d69188c915ea984f6eb8887f6a2d9  DDDDDDDD.txt",
				"792f1ac71da2ce43086f5fb02765a90e  EEEEEEEE.txt",
				"706d470269464276b0bb8a26c3417b1b  FFFFFFFF.txt",
				"a14f24c16fe9cb910dbd2aea9e14dc32  GGGGGGGG.txt",
				"da60f33aa1791392ddcde69d407a1e81  XXXXXXXX.txt",
				"39b0fb779fdfd5b64a9b57733a2b32f2  YYYYYYYY.txt",
				"98dcc0fe09e1d88e8168eaba815806a8  ZZZZZZZZ.txt"
			};

			File.WriteAllLines(filename, contents);
		}

		private void ExerciseOutputBuilders(Differences diff)
		{
			Assert.IsNotNull(diff);

			this.ExerciseHtmlBuilder(diff);
			this.ExerciseJsonBuilder(diff);
			this.ExerciseTextBuilder(diff);
			this.ExerciseXmlBuilder(diff);
			this.ExerciseXmlBuilder2(diff);
		}

		private void ExerciseHtmlBuilder(Differences diff)
		{
			Assert.IsNotNull(diff);

			using (var stream = new MemoryStream())
			using (var writer = new StreamWriter(stream))
			{
				var builder = new HtmlBuilder();
				builder.Build(writer, diff);

				Assert.IsTrue(writer.BaseStream.Length > 0);
			}
		}

		private void ExerciseJsonBuilder(Differences diff)
		{
			Assert.IsNotNull(diff);

			using (var stream = new MemoryStream())
			using (var writer = new StreamWriter(stream))
			{
				var builder = new JsonBuilder();
				builder.Build(writer, diff);

				Assert.IsTrue(writer.BaseStream.Length > 0);
			}
		}

		private void ExerciseTextBuilder(Differences diff)
		{
			Assert.IsNotNull(diff);

			using (var stream = new MemoryStream())
			using (var writer = new StreamWriter(stream))
			{
				var builder = new TextBuilder();
				builder.Build(writer, diff);

				Assert.IsTrue(writer.BaseStream.Length > 0);
			}
		}

		private void ExerciseXmlBuilder(Differences diff)
		{
			Assert.IsNotNull(diff);

			using (var stream = new MemoryStream())
			using (var writer = new StreamWriter(stream))
			{
				var builder = new XmlBuilder();
				builder.Build(writer, diff);

				Assert.IsTrue(writer.BaseStream.Length > 0);
			}
		}

		private void ExerciseXmlBuilder2(Differences diff)
		{
			Assert.IsNotNull(diff);

			using (var stream = new MemoryStream())
			using (var writer = new StreamWriter(stream))
			{
				var builder = new XmlBuilder2();
				builder.Build(writer, diff);

				Assert.IsTrue(writer.BaseStream.Length > 0);
			}
		}
	}
}