using Antlr4.Runtime;
using static JavaScriptParser;

namespace SmellFinder.Visitors.Interfaces
{
    public interface IBaseVisitor
    {
        string CustomVisit(ParserRuleContext ctx);
    }
}
