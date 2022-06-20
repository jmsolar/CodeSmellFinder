using Antlr4.Runtime;
using System;
using System.IO;

namespace SmellFinder.ErrorListeners
{
    public class CustomErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        // public static readonly bool REPORT_SYNTAX_ERRORS = true;
        // public static CustomErrorListener Instance { get; } = new CustomErrorListener();

        // public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
        //     if (!REPORT_SYNTAX_ERRORS) return;
        //     string sourceName = recognizer.InputStream.SourceName;
        //     // never ""; might be "<unknown>" == IntStreamConstants.UnknownSourceName
        //     sourceName = $"{sourceName}:{line}:{charPositionInLine}";
        //     Console.Error.WriteLine($"{sourceName}: line {line}:{charPositionInLine} {msg}");
        // }

        // public override void SyntaxError(TextWriter output, IRecognizer recognizer, Token offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
        //     this.SyntaxError(output, recognizer, 0, line, charPositionInLine, msg, e);
        // }

        // public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        // {
        //     throw new ArgumentException("Invalid Expression: {0}", msg, e);
        // }
        void IAntlrErrorListener<int>.SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new NotImplementedException();
        }
    }
}
