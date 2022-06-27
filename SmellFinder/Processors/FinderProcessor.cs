using Antlr4.Runtime.Tree;
using SmellFinder.Visitors.Implementations;
using SmellFinder.Visitors.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmellFinder.Processors
{
    public static class FinderProcessor
    {
        #region Methods
        public static Dictionary<string, string> Process(JavaScriptParser parser, List<string> smells)
        {
            var result = new Dictionary<string, string>();

            List<Type> visitors = new List<Type>
            {
                typeof(TernaryEvaluationVisitor),
                typeof(VarAssignmentVisitor)
            };

            foreach (var smell in smells)
            {
                var visitorName = visitors.Where(x => x.Name == smell).FirstOrDefault();
                var visitor = (BaseVisitor)Activator.CreateInstance(visitorName);

                IParseTree tree;
                tree = parser.program();

                if (parser.NumberOfSyntaxErrors > 0)
                {
                    continue;
                }

                visitor.Visit(tree);

                if (!string.IsNullOrEmpty(visitor.Smells()) && !result.ContainsKey(visitorName.Name))
                {
                    result.Add(visitorName.Name, visitor.Smells());
                }
            }

            return result;
        }
        #endregion
    }
}
