using SmellFinderTool.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SmellFinderTool.Utils
{
    public static class ReaderHelper
    {
        public static void ProcessDirectoryPath(string path, string smells)
        {
            if (File.Exists(path))
            {
                ProcessFile(path, smells);
            }
            else if (Directory.Exists(path))
            {
                ProcessDirectory(path, smells);
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }
        }
        public static void ProcessDirectory(string targetDirectory, string smells)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory, "*.js");
            foreach (string fileName in fileEntries)
            {
                ProcessFile(fileName, smells);
            }

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, smells);
        }

        public static void ProcessFile(string path, string smells)
        {
            var fileContent = LoadJS(path);
            var parser = SmellFinder.Utils.ParserGenerator.New(fileContent);
            SmellFinder.Processors.FinderProcessor.Process(parser, smells);

            Console.WriteLine("Processed file '{0}'.", path);
        }

        public static string LoadJS(string pathFile)
        {
            try
            {
                string path = Regex.Replace(Environment.CurrentDirectory, @"(bin.*)", "", RegexOptions.IgnoreCase) + pathFile;
                string fileContent = File.ReadAllText(path);

                return fileContent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
