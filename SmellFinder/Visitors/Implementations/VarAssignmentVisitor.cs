using SmellFinder.Attributes;
using SmellFinder.Visitors.Implementations.Base;
using System;

namespace SmellFinder.Visitors.Implementations
{
    [Visitor("VarAssignmentVisitor", Description = "Assignament value with 'var' sentence")]
    public class VarAssignmentVisitor : BaseVisitor
    {
        public override void EvalTreeNode(Antlr4.Runtime.ParserRuleContext ctx)
        {
            try
            {
                var varCTX = (JavaScriptParser.VariableDeclarationListContext)ctx;
                var stModifier = varCTX.varModifier();

                if (stModifier.GetText() == "var")
                {
                    var strPosition = stModifier.Start;
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
