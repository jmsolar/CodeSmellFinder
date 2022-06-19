using Antlr4.Runtime;

namespace SmellFinder.Visitors.Interfaces
{
    public interface IBaseVisitor
    {
        string CustomVisit(ParserRuleContext ctx);
    }
}
