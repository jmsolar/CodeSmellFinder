using SmellFinderTool.Progresses;
using SmellFinderTool.Utils;
using Spectre.Console;
using System.IO;

namespace SmellFinderTool
{
    public class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.Write(new Rule("[green]SmellFinderTool[/] v1.0 - 2022"));
            var path = CodeSmellSelector.SelectDirectory();
            var smells = CodeSmellSelector.SelectSmell();

            InitProcess(path, smells);

            // Proceso directorio
            // Creo directorio de salida
            // Genero un archivo json para informe haciendo append de la informacion obtenida
        }

        private static bool InitProcess(string path, string smells)
        {
            if (Directory.Exists(path))
            {
                AnsiConsole.MarkupLine("[yellow]Initializing process[/]...");
                CSProgress.Init(path, smells);
                return true;
            }

            AnsiConsole.Write(new Rule(string.Format("[red]Directory[/] {0} [red]path doesn't exist.[/]", path)));
            return false;
        }
    }
}
