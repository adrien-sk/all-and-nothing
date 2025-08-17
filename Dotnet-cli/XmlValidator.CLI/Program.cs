using System.CommandLine;
using XmlValidator.CLI.Commands;

namespace XmlValidator.CLI;

class Program {
	static int Main(string[] args) {
		//	Root command : used when there's no other argument (eg. checker.exe '<Code>hello world</Code>')
		RootCommand root = XmlRootCommand.Create();

		//	Add sub-commands
		root.Add(OtherCommand.Create());

		ParseResult parseResult = root.Parse(args);
		return parseResult.Invoke();
	}
}