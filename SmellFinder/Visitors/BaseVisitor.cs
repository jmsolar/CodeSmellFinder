using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace SmellFinder.Visitors
{
    public class BaseVisitor : JavaScriptParserBaseVisitor<string>
    {
        #region Fields
        private List<string> SmellPositionList { get; set; } = new List<string>();
        #endregion

        #region Methods
        public string BaseVisit(IParseTree tree) => Visit(tree);
        #endregion

        #region Helper methods
        public List<string> Smells() => SmellPositionList;

        public void AddSmell(string smell) => SmellPositionList.Add(smell);

        public bool Exists() => SmellPositionList.Count > 0;

        public string VisitNextTreeNode(ParserRuleContext ctx) => Visit(ctx.GetChild(0));
        #endregion
    }
}
