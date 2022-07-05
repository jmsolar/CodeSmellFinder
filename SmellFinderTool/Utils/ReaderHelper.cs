using SmellFinderTool.Progresses;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;

namespace SmellFinderTool.Utils
{
    public static class ReaderHelper
    {
        #region Private methods
        private static bool IsValidDirectory(string directory)
        {
            bool existPath = Directory.Exists(directory);

            if (!existPath)
            {
                AnsiConsole.Write(new Rule($"[red]Directory[/] {directory} [red]path doesn't exist[/]"));
            }

            return existPath;
        }
        #endregion

        #region Methods
        public static void ProcessFiles(string path, List<string> smells)
        {
            if (IsValidDirectory(path))
            {
                DisplayProgress.Init(path, smells);
            }

            AnsiConsole.MarkupLine("[grey]\nPress any key to close window[/]");
            AnsiConsole.Console.Input.ReadKey(true);
        }
        #endregion
    }
}
