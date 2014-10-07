namespace HashDiff
{
	using CommandLine;

	public class Options
	{
		[Option('1', "file1", Required = true, HelpText = "<filename> first file to compare")]
		public string File1 { get; set; }

		[Option('2', "file2", Required = true, HelpText = "<filename> second file to compare")]
		public string File2 { get; set; }

		[Option('e', "exitwitherrorondifference", Required = false, HelpText = "If a difference is found then exit with error 1")]
		public bool ExitWithErrorOnDifference { get; set; }

		[Option('i', "ignorecase", Required = false, HelpText = "Performs case-insensitive string comparison")]
		public bool IgnoreCase { get; set; }

		[Option('o', "outputfile", Required = false, HelpText = "Output filename")]
		public string OutputFile { get; set; }

		[Option('v', "verbose", Required = false, HelpText = "Displays verbose output")]
		public bool Verbose { get; set; }
	}
}