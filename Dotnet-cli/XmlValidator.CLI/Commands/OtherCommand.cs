using System.CommandLine;

namespace XmlValidator.CLI.Commands;

public static class OtherCommand {
	public static Command Create() {
		Command otherCommand = new("other", "Just an other command");

		otherCommand.SetAction(ParseResult => {
			Console.WriteLine("I'm the other command !");
		});

		return otherCommand;
	}
}
