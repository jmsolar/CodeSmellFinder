using SmellFinderTool.Progresses;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;

namespace SmellFinderTool.Utils
{
    public static class ReaderHelper
    {
        #region Private methods
        private static bool IsValidDirectory(string directory) => Directory.Exists(directory);
        #endregion

        #region Methods
        public static bool ProcessDirectoryPath(string path, List<string> smells)
        {
            if (IsValidDirectory(path))
            {
                AnsiConsole.MarkupLine("[yellow]Initializing process[/]...");

                CSProgress.Init(path, smells, out string filename);
                AnsiConsole.Write(new Rule(string.Format("[blue]See result details in file:[/] [italic]{0}[/]", filename)));

                return true;
            }

            AnsiConsole.Write(new Rule(string.Format("[red]Directory[/] {0} [red]path doesn't exist[/]", path)));
            return false;
        }
        #endregion
    }
}
