using SmellFinder.Attributes;
using System;
using static JavaScriptParser;

namespace SmellFinder.Visitors
{
    [Visitor("VarAssignmentVisitor", Description = "Assignament value with 'var' sentence", Message = "you should use keyword let or const for that")]
    public class VarAssignmentVisitor : BaseVisitor
    {
        #region Fields
        private static readonly string VarKeyword = "var";
        #endregion

        #region Methods
        public override string VisitVariableDeclarationList(VariableDeclarationListContext ctx)
        {
            try
            {
                var stModifier = ctx.varModifier();

                if (stModifier.GetText().Equals(VarKeyword))
                {
                    var strPosition = stModifier.Start;
                    var lineNumber = strPosition.Line.ToString();
                    AddSmell(string.Format("{0}", lineNumber));
                }

                return VisitNextTreeNode(ctx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
