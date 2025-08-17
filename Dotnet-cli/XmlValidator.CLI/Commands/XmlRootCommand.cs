using System.CommandLine;
using System.Text;

namespace XmlValidator.CLI.Commands;

public static class XmlRootCommand {
	public static RootCommand Create() {

		RootCommand root = new("Simple CLI to validate an XML input.");

		//	Add default argument (XML string)
		Argument<string> rootArgument = new("xml_string") {
			Arity = ArgumentArity.ExactlyOne,
			Description = "XML input to be validated."
		};

		//	Add File option flag
		Option<bool> fileOption = new("-f", "--file") {
			Description = "Flag to use a file as input instead of a string.\nExample :\tchecker -f my_file.xml"
		};

		root.Add(rootArgument);
		root.Add(fileOption);

		//	Root Command logic
		root.SetAction(parseResult => {
			//	Get first argument (XML string or File)
			var xml = parseResult.GetValue(rootArgument);

			//	If -f is present
			if(parseResult.GetValue(fileOption)) {
				//	Check for file
				if(!File.Exists(xml)) {
					Console.WriteLine("Error : File not found.");
					return;
				}

				xml = File.ReadAllText(xml).Replace("\n", "").Replace("\r", "").Trim();
			}

			//	Call Validation logic, and return result
			Console.WriteLine(
				ValidateXML(xml!) ?
					"Valid" :
					"Invalid"
			);
		});

		return root;
	}

	//	XML Validation logic
	private static bool ValidateXML(string xml) {
		//	False if doesn't start with "<" or end with ">"
		if(xml[0] != '<' || xml[^1] != '>') {
			return false;
		}

		var i = 0;
		var stack = new Stack<string>();

		while(i < xml.Length) {
			if(xml[i] == '<') {
				//	If opening tag : Extract Tag's string
				var tag = ExtractTag(xml[i..]);

				//	Never-closing tag
				if(string.IsNullOrEmpty(tag)) {
					return false;
				}

				if(tag[0] == '/') { //	Closing tag
					if(stack.Count == 0 || stack.Pop() != tag[1..]) {
						//	Doesn't correspond to last opening tag
						return false;
					}
				}
				else {  //	Opening tag
					stack.Push(tag);
				}

				i += tag.Length + 2;
			}
			else {
				i++;
			}
		}

		if(stack.Count > 0) {
			return false;
		}

		return true;
	}

	//	Returns the string between "<" and ">"
	//	or string.Empty if the tag is never closed by ">"
	private static string ExtractTag(string text) {
		StringBuilder sb = new();

		for(int i = 1; i < text.Length; i++) {
			if(text[i] == '>') {
				return sb.ToString();
			}

			sb.Append(text[i]);
		}

		return string.Empty;
	}
}
