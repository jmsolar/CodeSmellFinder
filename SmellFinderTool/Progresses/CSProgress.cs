using Newtonsoft.Json.Linq;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SmellFinderTool.Progresses
{
    public static class CSProgress
    {
        #region Fields
        private static Progress Progress { get; set; }
        private static string Filename { get; set; }
        private static List<string> Smells { get; set; }
        private static JArray Info { get; set; } = new JArray();
        #endregion

        #region Private methods
        private static void Config()
        {
            Progress = AnsiConsole
                .Progress()
                .AutoClear(false)
                .Columns(new ProgressColumn[]
                {
                    new TaskDescriptionColumn(),    // Task description
                    new ProgressBarColumn(),        // Progress bar
                    new PercentageColumn(),         // Percentage
                    new SpinnerColumn()             // Spinner
                });
        }

        private static List<(ProgressTask Task, int Delay)> CreateTasks(ProgressContext progress, Random random)
        {
            var tasks = new List<(ProgressTask, int)>
            {
                (progress.AddTask("Searching bad smells"), random.Next(2, 10))
            };

            return tasks;
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

        private static void ProcessFile(string path)
        {
            var fileContent = LoadJS(path);

            if (!string.IsNullOrEmpty(fileContent))
            {
                var parser = SmellFinder.Utils.ParserGenerator.New(fileContent);
                var smellsDetected = SmellFinder.Processors.FinderProcessor.Process(parser, Smells);
                
                JArray smellsArray = new JArray();
                foreach (var smell in smellsDetected)
                {
                    JObject fileProcessed = new JObject()
                    {
                        new JProperty("name", smell.Key),
                        new JProperty("linesAffected", smell.Value)
                    };

                    smellsArray.Add(fileProcessed);
                }

                Info.Add(smellsArray);
            }
        }

        private static string LoadJS(string pathFile)
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
        #endregion

        #region Methods
        public static void Init(string path, List<string> smells, out string filename)
        {
            Config();
            Filename = string.Concat(path, Path.DirectorySeparatorChar.ToString(), "BS-", DateTime.Now.ToString("yyyyMMddHHmmssffff"), ".json");
            Smells = smells;
            JObject data = new JObject();

            Progress.Start(ctx =>
            {
                var random = new Random(DateTime.Now.Millisecond);
                var tasks = CreateTasks(ctx, random);
                ProcessDirectory(path);

                while (!ctx.IsFinished)
                {
                    foreach (var (task, increment) in tasks)
                    {
                        task.Increment(random.NextDouble() * increment);
                    }

                    Thread.Sleep(100);
                }
            });

            data.Add(new JProperty("File", path));
            data.Add(new JProperty("Smells", Info));

            File.AppendAllText(Filename, data.ToString());

            filename = Filename;
        }
        #endregion
    }
}
