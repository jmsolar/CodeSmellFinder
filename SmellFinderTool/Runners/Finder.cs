using SmellFinderTool.Utils;
using Spectre.Console;
using System.Reflection;

namespace SmellFinderTool.Runners
{
    public static class Finder
    {
        public static void Run()
        {
            AnsiConsole.Write(new Rule($"[green bold]{Assembly.GetExecutingAssembly().GetName().Name}[/] [white bold]{Assembly.GetExecutingAssembly().GetName().Version}[/]"));
            ReaderHelper.ProcessFiles(DataSelector.SelectDirectory, DataSelector.SelectSmell());
        }
    }
}
