using SmellFinder.Attributes;
using System;
using static JavaScriptParser;

namespace SmellFinder.Visitors
{
    [Visitor("TernaryEvaluationVisitor", Description = "Evaluator ternary condition", Message = "you should use ternary condition for this purpose")]
    public class TernaryEvaluationVisitor : BaseVisitor
    {
        #region Methods
        public override string VisitIfStatement(IfStatementContext ctx)
        {
            try
            {
                var hasElseStatement = ctx.GetToken(Else, 0);

                if (!(hasElseStatement is null))
                {
                    var strPosition = ctx.Start;
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
