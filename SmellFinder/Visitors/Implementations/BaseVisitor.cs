using Antlr4.Runtime;
using SmellFinder.Visitors.Interfaces;
using System.Collections.Generic;

namespace SmellFinder.Visitors.Implementations
{
    public abstract class BaseVisitor : JavaScriptParserBaseVisitor<string>, IBaseVisitor
    {
        #region Fields
        private readonly List<string> SmellPositionList = new List<string>();
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
        public List<string> Smells() => this.SmellPositionList;

        public int SmellsFinder() => this.SmellPositionList.Count;

        public void AddSmell(string smell)
        {
            this.SmellPositionList.Add(smell);
        }
        #endregion
    }
}
