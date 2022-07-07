using Antlr4.Runtime.Tree;
using SmellFinder.Attributes;
using SmellFinder.Models;
using SmellFinder.Utils;
using SmellFinder.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmellFinder.Scanners
{
    public static class VisitorScanner
    {
        #region Private methods
        private static bool IsParsingValid(JavaScriptParser parser) => parser.NumberOfSyntaxErrors == 0;
        private static bool IsAttributeValid(Type type) => type.GetCustomAttributes(typeof(VisitorAttribute), true).Length > 0;
        private static List<Type> GetVisitors()
        {
            List<Type> visitors = new List<Type>();
            string definedIn = typeof(VisitorAttribute).Assembly.GetName().Name;

            foreach (var type in from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                                 where (!assembly.GlobalAssemblyCache) && ((assembly.GetName().Name == definedIn) || assembly.GetReferencedAssemblies().Any(a => a.Name == definedIn))
                                 from Type type in assembly.GetTypes()
                                 select type)
            {
                if (IsAttributeValid(type))
                {
                    VisitorAttribute attr = (VisitorAttribute)Attribute.GetCustomAttribute(type, typeof(VisitorAttribute));
                    visitors.Add(type);
                }
            }

            return visitors;
        }

        private static SmellResponse Visit(List<string> smells, JavaScriptParser parser)
        {
            SmellResponse foundSmells = new SmellResponse
            {
                LinesAffected = new List<string>()
            };

            IParseTree tree;
            tree = parser.program();

            List<Type> visitors = GetVisitors();

            foreach (var smell in smells)
            {
                var visitorName = visitors.Where(x => x.Name == smell).FirstOrDefault();
                var visitor = (BaseVisitor)Activator.CreateInstance(visitorName);

                if (!IsParsingValid(parser))
                {
                    continue;
                }

                visitor.BaseVisit(tree);

                if (visitor.Exists() && !foundSmells.Contains(visitorName.Name))
                {
                    var description = visitorName.CustomAttributes.Select(x => x.NamedArguments.Where(x => x.MemberName == "Description").First()).First();
                    var message = visitorName.CustomAttributes.Select(x => x.NamedArguments.Where(x => x.MemberName == "Message").First()).First();

                    foundSmells.Description = description.TypedValue.Value.ToString();
                    foundSmells.Message = message.TypedValue.Value.ToString();
                    foundSmells.LinesAffected.AddRange(visitor.Smells());
                }
            }

            return foundSmells;
        }
        #endregion

        #region Methods
        public static JavaScriptParser GetParser(string fileContent) => Parser.Create(fileContent);
        public static SmellResponse Search(List<string> smells, JavaScriptParser parser) => Visit(smells, parser);
        #endregion
    }
}
