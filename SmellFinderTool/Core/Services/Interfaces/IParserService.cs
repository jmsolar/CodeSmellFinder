namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IParserService
    {
         JavaScriptParser GenerateJSParser(string fileContent);
    }
}