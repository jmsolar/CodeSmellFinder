using System;
using System.Collections.Generic;
using System.Linq;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Interfaces;
using Spectre.Console;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class DisplayerService : IDisplayerService
    {
        #region Methods
        public void Init(AssemblyModel assembly)
        {
            ClearScreen();
            AnsiConsole.Write(
                new Rule(
                        string.Join(
                            " ",
                            $"{MessageFormatterModel.GetFormattedText("green bold", assembly.Name)}",
                            $"{MessageFormatterModel.GetFormattedText("white bold", assembly.Version.ToString())}"
                        )
                )
            );
        }

        public string AskDirectoryName() =>
            AnsiConsole.Ask<string>(
                $"\n\nWhich directory are you going to look for {MessageFormatterModel.GetFormattedText("springgreen4", "code smells")}?"
            );

        public void ClearScreen() => AnsiConsole.Clear();

        public void ShowDirectorySelected(string directoryName) =>
            AnsiConsole.MarkupLine(
                        string.Join(
                            " ",
                            $"{MessageFormatterModel.GetFormattedText("deepskyblue3_1", "\nThe directories and subdirectories of")}",
                            $"{MessageFormatterModel.GetFormattedText("gold3_1", directoryName)}",
                            $"{MessageFormatterModel.GetFormattedText("deepskyblue3_1", "would be analyzed")}"
                        )

            );

        public List<string> ShowMenu(Dictionary<string, string> options)
        {
            List<string> selected = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title($"What {MessageFormatterModel.GetFormattedText("springgreen4", "code smell")} do you need to validate?")
                    .NotRequired()
                    .PageSize(5)
                    .MoreChoicesText($"{MessageFormatterModel.GetFormattedText("grey", "(Move up and down to reveal more code smells)")}")
                    .InstructionsText(
                        string.Join(
                            " ",
                            $"{MessageFormatterModel.GetFormattedText("grey", "(Press")}",
                            $"{MessageFormatterModel.GetFormattedText("blue", "<space>")}",
                            $"{MessageFormatterModel.GetFormattedText("grey", "to toggle a code smell,")}",
                            $"{MessageFormatterModel.GetFormattedText("green", "<enter>")}",
                            $"{MessageFormatterModel.GetFormattedText("gray", "to accept and begin process)")}"
                        )
                    )
                    .AddChoiceGroup("All", options.Values)
            );

            if (selected.Any())
            {
                ShowOptionsSelected(selected);

                return ConvertSmellToVisitor(options, selected);
            }

            AnsiConsole.MarkupLine(
                string.Join(
                    " ",
                    $"{MessageFormatterModel.GetFormattedText("white on yellow", "\n WARN ")}",
                    $"{MessageFormatterModel.GetFormattedText("yellow", "Should be select a option move on")}"
                )
            );

            return selected;
        }

        public void ShowProgress(Action[] tasks)
        {
            AnsiConsole
                .Status()
                .Start($"{MessageFormatterModel.GetFormattedText("yellow", "Searching bad smells...")}", ctx =>
                {
                    foreach (var task in tasks)
                    {
                        task.Invoke();
                    }
                });
        }

        public void ShowErrorByNonExistDirectory(string directoryName) =>
            AnsiConsole.MarkupLine(
                string.Join(
                    " ",
                    $"{MessageFormatterModel.GetFormattedText("white on red", "\n\n FAIL ")}",
                    $"{directoryName}",
                    $"{MessageFormatterModel.GetFormattedText("red", "path doesn't exist")}"
                )
            );

        public void ShowException(Exception ex) => AnsiConsole.WriteException(ex);

        public void Exit()
        {
            AnsiConsole.MarkupLine($"\n{MessageFormatterModel.GetFormattedText("grey", "Press any key to close window")}");
            AnsiConsole.Console.Input.ReadKey(true);
            ClearScreen();
        }

        public void ShowCounterOfFiles(int numberOfFiles)
        {
            var message = "Not found files to be processed";

            if (numberOfFiles > 0)
                message = $"There are {numberOfFiles} files to be processed";

            AnsiConsole.MarkupLine($"\n{MessageFormatterModel.GetFormattedText("blue on white bold", message)}");
        }

        public void ShowEndOfProcess(string fileNameOutput) =>
            AnsiConsole.MarkupLine(
                string.Join(
                    " ",
                    $"{MessageFormatterModel.GetFormattedText("white on springgreen4", "\n\n OK ")}",
                    $"{MessageFormatterModel.GetFormattedText("yellow", "Review the result in the output file")}",
                    $"{MessageFormatterModel.GetFormattedText("white italic", fileNameOutput)}"
                )
            );
        #endregion

        #region  Private methods
        private void ShowOptionsSelected(List<string> optionsSelected)
        {
            AnsiConsole.MarkupLine($"{MessageFormatterModel.GetFormattedText("deepskyblue3_1", "You selected the following options")}");

            foreach (var option in optionsSelected)
            {
                AnsiConsole.MarkupLine($"* {MessageFormatterModel.GetFormattedText("gold3_1 italic", option)}");
            }
        }

        private List<string> ConvertSmellToVisitor(Dictionary<string, string> options, List<string> selected)
        {
            return options
                .Where(x => selected.Contains(x.Value))
                .Select(x => x.Key)
                .ToList();
        }
        #endregion
    }
}