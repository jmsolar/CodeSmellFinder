using Newtonsoft.Json.Linq;
using SmellFinder.Scanners;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmellFinderTool.Utils
{
    public static class FileManager
    {
        #region Fields
        private static List<string> Smells { get; set; }
        private static JArray Data { get; set; } = new JArray();
        private static JavaScriptParser Parser { get; set; }
        #endregion

        #region Methods
        public static string GetFileName(string path) => string.Concat(path, Path.DirectorySeparatorChar.ToString(), "BS-", DateTime.Now.ToString("yyyyMMddHHmmssffff"), ".json");

        public static string Load(string pathFile)
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

        public static JArray Run(List<string> smells, string path)
        {
            Smells = smells;
            ProcessDirectory(path);

            return Data;
        }
        #endregion

        #region Private methods
        private static void ProcessFile(string path)
        {
            string fileContent = Load(path);
            Parser = VisitorScanner.GetParser(fileContent);

            if (!string.IsNullOrEmpty(fileContent))
            {
                Dictionary<string, List<string>> smellsDetected = VisitorScanner.Search(Smells, Parser);

                JArray smellsArray = new JArray();
                foreach (var smell in smellsDetected)
                {
                    JObject smellProcessed = new JObject()
                    {
                        new JProperty("name", smell.Key),
                        new JProperty("linesAffected", smell.Value)
                    };

                    smellsArray.Add(smellProcessed);
                }

                if (smellsArray.Any())
                {
                    JObject fileProcessed = new JObject()
                    {
                        new JProperty("file", path),
                        new JProperty("smells", smellsArray)
                    };

                    Data.Add(fileProcessed);
                }
            }
        }

        private static void ProcessDirectory(string targetDirectory)
        {
            List<string> fileToProcess = Directory.GetFiles(targetDirectory).Where(x => x.EndsWith(".js") && !x.EndsWith(".min.js")).ToList();
            foreach (string fileName in fileToProcess)
            {
                try
                {
                    ProcessFile(fileName);
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
                    ProcessDirectory(subdirectory);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
            }
        }
        #endregion
    }
}