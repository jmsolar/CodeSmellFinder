using System;

namespace SmellFinder.Visitors.Implementations
{
    public class TernaryEvaluationVisitor : BaseVisitor
    {
        public override void EvalTreeNode(Antlr4.Runtime.ParserRuleContext ctx)
        {
            try
            {
                var ifCTX = (JavaScriptParser.IfStatementContext)ctx;
                var hasElseStatement = ifCTX.GetToken(JavaScriptParser.Else, 0);

                if (!(hasElseStatement is null))
                {
                    var strPosition = ifCTX.Start;
                    var lineNumber = strPosition.Line.ToString();
                    base.AddSmell(string.Format("{0}", lineNumber));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
