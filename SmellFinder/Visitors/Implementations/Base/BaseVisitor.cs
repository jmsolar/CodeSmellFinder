using Antlr4.Runtime;
using SmellFinder.Visitors.Interfaces;

namespace SmellFinder.Visitors.Implementations.Base
{
    public abstract class BaseVisitor : JavaScriptParserBaseVisitor<string>, IBaseVisitor
    {
        #region Fields
        private string SmellPositionList { get; set; } = string.Empty;
        #endregion

        #region Methods
        public string CustomVisit(ParserRuleContext ctx)
        {
            EvalTreeNode(ctx);
            return VisitNextTreeNode(ctx);
        }
        #endregion

        #region Steps template methods
        public abstract void EvalTreeNode(ParserRuleContext ctx);

        private string VisitNextTreeNode(ParserRuleContext ctx) => Visit(ctx.GetChild(0));
        #endregion

        #region Helper methods
        public string Smells() => SmellPositionList;

        public void AddSmell(string smell)
        {
            this.SmellPositionList = string.Concat(Smells(), ",", smell);
        }
        #endregion
    }
}
