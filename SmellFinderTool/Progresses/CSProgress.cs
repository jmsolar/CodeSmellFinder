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
        private static Progress Progress { get; set; }

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
                    new RemainingTimeColumn(),      // Remaining time
                    new SpinnerColumn()             // Spinner
                });
        }

        private static List<(ProgressTask Task, int Delay)> CreateTasks(ProgressContext progress, Random random)
        {
            var tasks = new List<(ProgressTask, int)>();
            while (tasks.Count < 5)
            {
                if (DescriptionGenerator.TryGenerate(out var name))
                {
                    tasks.Add((progress.AddTask(name), random.Next(2, 10)));
                }
            }

            return tasks;
        }

        private static void ProcessDirectory(string targetDirectory, string smells)
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

        private static void ProcessFile(string path, string smells)
        {
            var fileContent = LoadJS(path);
            var parser = SmellFinder.Utils.ParserGenerator.New(fileContent);
            SmellFinder.Processors.FinderProcessor.Process(parser, smells);

            AnsiConsole.Write("[green]Processed file[/] {0}", path);
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

        public static void Init(string path, string smells)
        {
            Config();
            Progress.Start(ctx =>
            {
                var random = new Random(DateTime.Now.Millisecond);

                // Create some tasks
                var tasks = CreateTasks(ctx, random);
                var warpTask = ctx.AddTask("Going to warp", autoStart: false).IsIndeterminate();

                // Wait for all tasks (except the indeterminate one) to complete
                while (!ctx.IsFinished)
                {
                    // Increment progress
                    foreach (var (task, increment) in tasks)
                    {
                        task.Increment(random.NextDouble() * increment);
                    }

                    Thread.Sleep(1);
                }

                // Now start the "warp" task
                warpTask.StartTask();
                warpTask.IsIndeterminate(false);
                while (!ctx.IsFinished)
                {
                    warpTask.Increment(12 * random.NextDouble());
                    Thread.Sleep(1);
                }
            });
        }
    }
}
