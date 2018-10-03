# HashDiff

<img align="right" src="docs/dotnet-hashdiff.png">

[![NuGet release](https://img.shields.io/nuget/v/HashDiff.Core.svg)](https://www.nuget.org/packages/HashDiff.Core)

Use the HashDiff tool when you need to compare the contents of two sets of checksum hashes.

Run it as a standalone executable. The tool supports three output formats: plain text, XML and HTML.

If you would like more information about checksum hashes, please see the [Wikipedia article on md5sum](http://en.wikipedia.org/wiki/Md5sum).

## Supported Platforms

* .NET 4.5 (Desktop / Server)


## Getting Started

### Installation

[HashDiff is available on NuGet](https://www.nuget.org/packages/HashDiff).

	Install-Package HashDiff

For the underlying class library, use [HashDiff.Core](https://www.nuget.org/packages/HashDiff.Core).

	Install-Package HashDiff.Core


### Build

If you prefer, you can compile HashDiff yourself, you'll need:

* Visual Studio 2012 (or above)

To clone it locally click the "Clone in Windows" button above or run the following git commands.

	git clone https://github.com/dotnet-zipdiff-utils/hashdiff.git hashdiff
	cd hashdiff
	.\build.cmd


## Command line arguments

	hashdiff.exe --file1 foo.md5 bar.md5 [--options]

Valid options are:

	--file1                       <filename> first file to compare
	--file2                       <filename> second file to compare
	--ignorecase                  Performs case-insensitive string comparison on the file name
	--outputfile                  Name of the output file
	--exitwitherrorondifference   Use an error code other than 0, if differences have been detected
	--verbose                     Print detail messages

### Output formats

When using the `--outputfile` option, the following formats are available:

* Plain-Text
* HTML
* XML

For example, to output an XML file:

	hashdiff.exe --file1 foo.md5 --file2 bar.md5 --outputfile diffs.xml

If the output file extension can not be determined, then the format will default to a plain-text file.


## Class library usage

To compare the differences between 2 sets of MD5 checksum hashes:

	var calc = new HashDiff.Core.DifferenceCalculator("foo.md5", "bar.md5");
	var diff = calc.GetDifferences();

	if (diff.HasDifferences())
	{
		Console.WriteLine("Added: {0}", diff.Added.Count);
		Console.WriteLine("Removed: {0}", diff.Removed.Count);
		Console.WriteLine("Changed: {0}", diff.Changed.Count);
	}


## Contact

Have a question?

* [Raise an issue](https://github.com/dotnet-zipdiff-utils/hashdiff/issues) on GitHub


## Contributing to this project

Anyone and everyone is welcome to contribute. Please take a moment to review the [guidelines for contributing](CONTRIBUTING.md).

* [Bug reports](CONTRIBUTING.md#bugs)
* [Feature requests](CONTRIBUTING.md#features)
* [Pull requests](CONTRIBUTING.md#pull-requests)


## Copyright and License

Copyright &copy; 2016 Lee Kelleher, Umbrella Inc Ltd

This project is licensed under [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0).

Please see [LICENSE](LICENSE.md) for further details.
