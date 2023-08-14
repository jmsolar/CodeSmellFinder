using Antlr4.Runtime;

namespace SmellFinder.Utils
{
    public static class Parser
    {
        public static JavaScriptParser Create(string fileContent)
        {
            var lexer = new JavaScriptLexer(new AntlrInputStream(fileContent));
            lexer.RemoveErrorListeners();
            var parser = new JavaScriptParser(new CommonTokenStream(lexer));
            parser.RemoveErrorListeners();

            return parser;
        }
    }
}