﻿using Spectre.Console;
using System;
using System.IO;

namespace SmellFinderTool.Utils
{
    public static class CodeSmellSelector
    {
        public static string SelectDirectory()
        {
            var directoryName = AnsiConsole.Ask<string>("Which directory are you going to look for [green]code smells[/]?");

            return directoryName;
        }

        public static string SelectSmell()
        {
            var smells = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                            .Title("What [green]code smell[/] do you need to validate?")
                            .NotRequired()
                            .PageSize(5)
                            .MoreChoicesText("[grey](Move up and down to reveal more code smells)[/]")
                            .InstructionsText(
                                "[grey](Press [blue]<space>[/] to toggle a code smell, " +
                                "[green]<enter>[/] to accept and process)[/]")
                            .AddChoices(new[] { "Assignament by 'var' sentence", "Ternary operator" }));

            foreach (string smell in smells) AnsiConsole.WriteLine(smell);

            var smellsSelected = smells.Count == 1 ? smells[0] : null;

            return smellsSelected;
        }
    }
}
