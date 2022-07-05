using SmellFinder.Attributes;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmellFinderTool.Utils
{
    public static class DataSelector
    {
        #region Private methods
        private static Dictionary<string, string> GetOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            string definedIn = typeof(VisitorAttribute).Assembly.GetName().Name;

            foreach (var type in from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                                 where (!assembly.GlobalAssemblyCache) && ((assembly.GetName().Name == definedIn) || assembly.GetReferencedAssemblies().Any(a => a.Name == definedIn))
                                 from Type type in assembly.GetTypes()
                                 select type)
            {
                try
                {
                    if (type.GetCustomAttributes(typeof(VisitorAttribute), true).Length > 0)
                    {
                        VisitorAttribute attr = (VisitorAttribute)Attribute.GetCustomAttribute(type, typeof(VisitorAttribute));
                        options.Add(attr.Name, attr.Description);
                    }
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine("\nHey! you should need review you code.." + ex.Message);
                    continue;
                }
            }

            return options;
        }

        private static Dictionary<string, string> Options { get => GetOptions(); }

        private static List<string> ConvertSmellToVisitor(List<string> selected) => Options.Where(x => selected.Contains(x.Value)).Select(x => x.Key).ToList();
        #endregion

        #region Methods
        public static string SelectDirectory => AnsiConsole.Ask<string>("\n\nWhich directory are you going to look for [springgreen4]code smells[/]?");

        public static List<string> SelectSmell()
        {
            List<string> selected = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                            .Title("What [springgreen4]code smell[/] do you need to validate?")
                            .NotRequired()
                            .PageSize(5)
                            .MoreChoicesText("[grey](Move up and down to reveal more code smells)[/]")
                            .InstructionsText(
                                "[grey](Press [blue]<space>[/] to toggle a code smell, " +
                                "[green]<enter>[/] to accept and process)[/]")
                            .AddChoiceGroup("All", Options.Values));

            AnsiConsole.MarkupLine("[deepskyblue3_1]Your selected options:[/] [bold italic]{0}[/]", string.Join(", ", selected));

            return ConvertSmellToVisitor(selected);
        }
        #endregion
    }
}
