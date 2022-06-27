using SmellFinderTool.Utils;
using Spectre.Console;

namespace SmellFinderTool.Runner
{
    public static class Executor
    {
        public static void Run()
        {
            var version = "1.0";
            AnsiConsole.Write(new Rule(string.Format("[green]SmellFinderTool[/] {0}", version)));
            var path = DataSelector.SelectDirectory();
            var smells = DataSelector.SelectSmell();

            ReaderHelper.ProcessDirectoryPath(path, smells);
        }
    }
}
