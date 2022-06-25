using SmellFinderTool.Progresses;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmellFinderTool.Utils
{
    public static class ReaderHelper
    {
        public static bool ProcessDirectoryPath(string path, string smells)
        {
            if (Directory.Exists(path))
            {
                AnsiConsole.MarkupLine("[yellow]Initializing process[/]...");
                CSProgress.Init();
                ProcessDirectory(path, smells);
                return true;
            }

            AnsiConsole.Write(new Rule(string.Format("[red]Directory[/] {0} [red]path doesn't exist.[/]", path)));
            return false;
        }

        public static void ProcessDirectory(string targetDirectory, string smells)
        {
            List<string> fileToProcess = Directory.GetFiles(targetDirectory).Where(x => x.EndsWith(".js") && !x.EndsWith(".min.js")).ToList();

            foreach (string fileName in fileToProcess)
            {
                try
                {
                    ProcessFile(fileName, smells);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                try
                {
                    ProcessDirectory(subdirectory, smells);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
            }
        }

        public static void ProcessFile(string path, string smells)
        {
            var fileContent = LoadJS(path);
            var parser = SmellFinder.Utils.ParserGenerator.New(fileContent);
            SmellFinder.Processors.FinderProcessor.Process(parser, smells);

            AnsiConsole.Write("[green]Processed file[/] {0}", path);
        }

        public static string LoadJS(string pathFile)
        {
            try
            {
                return File.ReadAllText(pathFile);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
