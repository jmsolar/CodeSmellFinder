using SmellFinder.Attributes;
using System;
using static JavaScriptParser;

namespace SmellFinder.Visitors
{
    [Visitor("ObjectInstanceVisitor", Description = "Creation object")]
    public class ObjectInstanceVisitor : BaseVisitor
    {
        #region Fields
        private static readonly string ObjectKeyword = "Object";
        private static readonly string ArrayKeyword = "Array";
        #endregion

        #region Methods
        public override string VisitNewExpression(NewExpressionContext ctx)
        {
            try
            {
                string identifier = ctx.singleExpression().GetText();
                var arguments = ctx.arguments().argument().Length > 0;

                if ((identifier.Equals(ObjectKeyword) || identifier.Equals(ArrayKeyword)) && !arguments)
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