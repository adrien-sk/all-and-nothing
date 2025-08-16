using System.CommandLine;
using XmlValidator.CLI.Commands;

namespace XmlValidator.UnitTests;

public class XmlRootCommandTest {
	public static IEnumerable<object[]> TestData() {
		yield return new object[] { new string[] { "<Design><Code>hello world</Code></Design>" }, "Valid" };
		yield return new object[] { new string[] { "<De<sign><Code>hello world</Code></De<sign>" }, "Valid" };
		yield return new object[] { new string[] { "<Code>hello world</Code>" }, "Valid" };
		yield return new object[] { new string[] { "<Design><Code>hello world</Code><Visual>Studio</Visual></Design>" }, "Valid" };

		yield return new object[] { new string[] { "hello <Design><Code>hello world</Code></Design>" }, "Invalid" };
		yield return new object[] { new string[] { "<Design><Code>hello world</Code></Design>hello" }, "Invalid" };
		yield return new object[] { new string[] { "<Design><Code>hello world</Code></Design><People>" }, "Invalid" };
		yield return new object[] { new string[] { "<People><Design><Code>hello world</People></Code></Design>" }, "Invalid" };
	}

	[Theory]
	[MemberData(nameof(TestData))]
	public void Cli_Should_ReturnExpectedOutput(string[] args, string expected) {
		RootCommand root = XmlRootCommand.Create();

		using var sw = new StringWriter();
		Console.SetOut(sw);

		ParseResult parseResult = root.Parse(args);
		parseResult.Invoke();

		var output = sw.ToString().Trim();
		Assert.Equal(expected, output);
	}
}
