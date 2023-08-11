using SmellFinder.Utils;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class ParserService : IParserService
    {
        public JavaScriptParser GenerateJSParser(string fileContent) => Parser.Create(fileContent);
    }
}