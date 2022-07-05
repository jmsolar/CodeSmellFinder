using Newtonsoft.Json.Linq;
using SmellFinderTool.Utils;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;

namespace SmellFinderTool.Progresses
{
    public static class DisplayProgress
    {
        #region Methods
        public static void Init(string path, List<string> smells)
        {
            string filename = FileManager.GetFileName(path);

            AnsiConsole
                .Status()
                .Start("[yellow]Searching bad smells...[/]", ctx =>
                {
                    JArray Data = FileManager.Run(smells, path);

                    AnsiConsole.MarkupLine("[springgreen3 bold]Saving output file...[/]");
                    File.AppendAllText(filename, Data.ToString());
                });

            AnsiConsole.MarkupLine($"[springgreen4 on white] OK [/][yellow] view result in the output file: [/][white italic]{filename}[/]");
        }
        #endregion
    }
}
