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
        #region Fields
        private readonly IResourceService _textResource;
        private ReportSettings _reportSettings;
        private readonly ExceptionSettings _exceptionSettings = new ExceptionSettings
        {
            Format = ExceptionFormats.ShortenEverything,
            Style = new ExceptionStyle
            {
                Exception = new Style().Foreground(Color.Red),
                Message = new Style().Foreground(Color.White),
                NonEmphasized = new Style().Foreground(Color.White),
                Parenthesis = new Style().Foreground(Color.White),
                Method = new Style().Foreground(Color.Red),
                ParameterName = new Style().Foreground(Color.White),
                ParameterType = new Style().Foreground(Color.Red),
                Path = new Style().Foreground(Color.Red),
                LineNumber = new Style().Foreground(Color.White),
            }
        };
        private readonly int _pageSizeMenu = 4;

        /* MESSAGES */
        private string _progressMsg;
        private string _titleSmellSelectorMsg;
        private string _spaceText;
        private string _enterText;
        private string _instructionsText;
        private string _moreChoiceMsg;
        private string _all;
        private string _titleReportSelectorMsg;
        private string _outputFileExtensionMsg;
        private char _yes;
        private char _no;
        private Dictionary<TaskStatusModel, List<string>> _statusMsg;
        #endregion

        #region Constructors
        public DisplayerService(IResourceService resourceService, ReportSettings reportSettings)
        {
            _textResource = resourceService;
            _reportSettings = reportSettings;
        }
        #endregion

        #region Methods
        public void Init()
        {
            _progressMsg = MessageFormatterModel.GetFormattedText("yellow", _textResource.GetStringResource("progress"));
            _titleSmellSelectorMsg = _textResource.GetStringResource("askSmellToValidate", new List<string> { MessageFormatterModel.GetFormattedText("springgreen4", "code smells") });
            _spaceText = MessageFormatterModel.GetFormattedText("blue", _textResource.GetStringResource("selectorKey"));
            _enterText = MessageFormatterModel.GetFormattedText("springgreen4", _textResource.GetStringResource("continueKey"));
            _instructionsText = MessageFormatterModel.GetFormattedText("gray", _textResource.GetStringResource("smellsInstructions", new List<string> { _spaceText, _enterText }));
            _moreChoiceMsg = MessageFormatterModel.GetFormattedText("gray", _textResource.GetStringResource("menuChoiceText"));
            _all = _textResource.GetStringResource("all");

            _titleReportSelectorMsg = _textResource.GetStringResource("chooseExtensionFile");
            _outputFileExtensionMsg = MessageFormatterModel.GetFormattedText("deepskyblue3_1", _textResource.GetStringResource("outputFileExtensionSelected"));
        
            _statusMsg = new Dictionary<TaskStatusModel, List<string>>
            {
                { TaskStatusModel.SUCCESS, new List<string> { MessageFormatterModel.GetFormattedText("white on green bold", _textResource.GetStringResource("success")), "green" } },
                { TaskStatusModel.INFO, new List<string> { MessageFormatterModel.GetFormattedText("white on blue bold", _textResource.GetStringResource("info")), "blue" } },
                { TaskStatusModel.WARN, new List<string> { MessageFormatterModel.GetFormattedText("white on yellow bold", _textResource.GetStringResource("warn")), "yellow" } },
                { TaskStatusModel.FAIL, new List<string> { MessageFormatterModel.GetFormattedText("white on red bold", _textResource.GetStringResource("error")), "red" } }
            };

            _yes = char.Parse(_textResource.GetStringResource("yes"));
            _no = char.Parse(_textResource.GetStringResource("no"));
        }

        public void ShowStatus(TaskStatusModel taskStatus, string message)
        {
            if (_statusMsg.TryGetValue(taskStatus, out List<string> statusMessage)) {
                message = MessageFormatterModel.GetFormattedText(statusMessage[1], message);
                ShowSimpleMessage($"\n{message.Replace("{0}", statusMessage[0])}");
            }
        }

        public void ShowAppInfo(string message) => AnsiConsole.Write(new Rule(message));

        public string ShowAskMessage(string message) => AnsiConsole.Ask<string>(message);

        public bool ShowConfirmMessage(string message) {
            var prompt = new ConfirmationPrompt(message);
            prompt.Yes(char.Parse(_textResource.GetStringResource("yes")));
            prompt.No(char.Parse(_textResource.GetStringResource("no")));
            return prompt.Show(AnsiConsole.Console);
        }

        public void ShowSimpleMessage(string message) => AnsiConsole.MarkupLine(message);

        public void ClearScreen() => AnsiConsole.Clear();

        public List<SmellReportedModel> ShowProgress(Func<List<SmellReportedModel>> task)
        {
            var result = new List<SmellReportedModel>();

            AnsiConsole
                .Status()
                .AutoRefresh(true)
                .Start(_progressMsg, ctx =>
                {
                    result = task.Invoke();
                });

            return result;
        }

        public List<string> ShowMenu(Dictionary<string, string> options)
        {
            List<string> selected = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title(_titleSmellSelectorMsg)
                    .Required()
                    .PageSize(_pageSizeMenu)
                    .MoreChoicesText(_moreChoiceMsg)
                    .InstructionsText(_instructionsText)
                    .AddChoiceGroup(_all, options.Values)
            );

            if (selected.Any())
            {
                ShowOptionsSelected(selected);
                return ConvertSmellToVisitor(options, selected);
            }

            ShowStatus(TaskStatusModel.WARN, _textResource.GetStringResource("selectOption"));

            return selected;
        }

        public string ShowOutputExtension()
        {
            var optionSelected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(_titleReportSelectorMsg)
                    .PageSize(_pageSizeMenu)
                    .MoreChoicesText(_moreChoiceMsg)
                    .AddChoices(_reportSettings.FileExtension.Select(x => x.Description).ToList())
            );

            var fileExtensionSelected = ConvertDescriptionToFileExtension(_reportSettings.FileExtension, optionSelected);
            ShowSimpleMessage(string.Format(_outputFileExtensionMsg, MessageFormatterModel.GetFormattedText("gold3_1 italic", optionSelected)));

            return fileExtensionSelected;
        }

        public void ShowException(Exception ex) => AnsiConsole.WriteException(ex, _exceptionSettings);
        #endregion

        #region  Private methods
        private void ShowOptionsSelected(List<string> optionsSelected)
        {
            ShowSimpleMessage(MessageFormatterModel.GetFormattedText("deepskyblue3_1", "\n" + _textResource.GetStringResource("optionsSelected")));

            foreach (var option in optionsSelected)
            {
                ShowSimpleMessage($"* {MessageFormatterModel.GetFormattedText("gold3_1 italic", option)}");
            }

            ShowSimpleMessage("");
        }

        private List<string> ConvertSmellToVisitor(Dictionary<string, string> options, List<string> selected) =>
            options
                .Where(x => selected.Contains(x.Value))
                .Select(x => x.Key)
                .ToList();

        private string ConvertDescriptionToFileExtension(List<FileExtension> options, string selected) =>
            options
                .Where(x => selected.Contains(x.Description))
                .Select(x => x.Type).FirstOrDefault();
        #endregion
    }
}