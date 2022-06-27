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
        private static Dictionary<string, string> GetOptionsName()
        {
            var resp = new Dictionary<string, string>();

            Assembly objAssembly = Assembly.Load("SmellFinder, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            string definedIn = typeof(VisitorAttribute).Assembly.GetName().Name;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if ((!assembly.GlobalAssemblyCache) && ((assembly.GetName().Name == definedIn) || assembly.GetReferencedAssemblies().Any(a => a.Name == definedIn)))
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        try
                        {
                            if (type.GetCustomAttributes(typeof(VisitorAttribute), true).Length > 0)
                            {
                                VisitorAttribute attr = (VisitorAttribute)Attribute.GetCustomAttribute(type, typeof(VisitorAttribute));
                                resp.Add(attr.Name, attr.Description);
                            }
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.WriteLine("Hey! you should need review you code.." + ex.ToString());
                            continue;
                        }
                    }
                }
            }

            return resp;
        }
        private static Dictionary<string, string> options { get => GetOptionsName(); }
        #endregion

        #region Methods
        public static string SelectDirectory() => AnsiConsole.Ask<string>("Which directory are you going to look for [green]code smells[/]?");

        public static List<string> SelectSmell()
        {
            AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                            .Title("What [green]code smell[/] do you need to validate?")
                            .NotRequired()
                            .PageSize(5)
                            .MoreChoicesText("[grey](Move up and down to reveal more code smells)[/]")
                            .InstructionsText(
                                "[grey](Press [blue]<space>[/] to toggle a code smell, " +
                                "[green]<enter>[/] to accept and process)[/]")
                            .AddChoiceGroup("All", options.Values));

            AnsiConsole.MarkupLine("[blue]Your selected options:[/] {0}", string.Join(", ", options.Values));

            return options.Keys.ToList();
        }
        #endregion
    }
}
