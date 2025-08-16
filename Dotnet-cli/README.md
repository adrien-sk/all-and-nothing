dotnet new sln -n XmlValidator
dotnet new console -n XmlValidator.CLI --use-program-main
dotnet sln add .\XmlValidator.CLI\XmlValidator.CLI.csproj
dotnet add package System.CommandLine --prerelease
Updated csproj for single file publish
