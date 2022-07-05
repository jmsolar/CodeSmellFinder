using Antlr4.Runtime;
using SmellFinder.ErrorListeners;

namespace SmellFinder.Utils
{
    public static class Parser
    {
        public static JavaScriptParser Create(string fileContent)
        {
            var stream = new AntlrInputStream(fileContent);
            var lexer = new JavaScriptLexer(stream);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(new CustomErrorListener());
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new JavaScriptParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new CustomErrorListener());

            return parser;
        }
    }
}