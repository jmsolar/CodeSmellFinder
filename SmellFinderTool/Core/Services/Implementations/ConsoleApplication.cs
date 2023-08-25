using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Implementations.ReportWriterStrategies;
using SmellFinderTool.Core.Services.Interfaces;
using System.Collections.Generic;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class ConsoleApplication
    {
        #region Fields
        private readonly IDisplayerService _displayer;
        private readonly IAnalizerService _analizer;
        private readonly IFileSystemManagerService _fileSystemManager;
        private readonly AssemblyModel _assembly;
        private IReportWriterStrategy _reportWriter;
        private readonly IResourceService _textResource;
        private readonly LanguageSettings _languageSettings;
        private string _message;
        private string _directoryName;
        private List<string> _filesToProcess;
        private List<string> _smellsToAnalyze;
        private string _outputExtension;
        private string _filenameOutput;
        private string _reprocess;
        private List<SmellReportedModel> _listOfSmells;
        #endregion

        #region Constructors
        public ConsoleApplication(IDisplayerService displayer, IAnalizerService analizer, IFileSystemManagerService fileSystemManager,
            LanguageSettings languageSettings, IResourceService resourceService
        )
        {
            _displayer = displayer;
            _analizer = analizer;
            _fileSystemManager = fileSystemManager;
            _textResource = resourceService;
            _languageSettings = languageSettings;
            _assembly = new AssemblyModel(
                    name: Assembly.GetExecutingAssembly().GetName().Name,
                    version: Assembly.GetExecutingAssembly().GetName().Version
            );
        }
        #endregion

        #region Methods
        public async Task Run()
        {
            Init();
            bool follow = true;

            while (follow)
            {
                try
                {
                    do
                    {
                        ShowAppInfo();
                        ShowDirectorySelector();
                        _filesToProcess = await _fileSystemManager.FilesToProcess(_directoryName);
                        _smellsToAnalyze = ShowCodeSmellsSelector();

                        if (_smellsToAnalyze.Any())
                        {
                            ShowReportTypeSelector();
                            ShowTotalFilesToProcess();

                            if (_filesToProcess.Count > 0) {
                                _filenameOutput = _fileSystemManager.FilenameOutput(_directoryName, _outputExtension);

                                SearchCodeSmells();
                                WriteReport();
                                ShowProcessResult();
                            }
                        }

                        follow = _displayer.ShowConfirmMessage(_reprocess);

                        if (follow) _displayer.ClearScreen();
                    } while (follow);
                }
                catch (Exception ex)
                {
                    _displayer.ShowException(ex);
                    follow = _displayer.ShowConfirmMessage(_reprocess);
                }
            }
        }
        #endregion

        #region Private methods
        private void Init()
        {
            try
            {
                _textResource.SetLanguage(_languageSettings.ISOCode);
                _reprocess = _textResource.GetStringResource("reprocess");
                _displayer.Init();
            }
            catch (Exception ex)
            {
                _displayer.ShowException(ex);
            }
        }

        private void ShowAppInfo()
        {
            _displayer.ClearScreen();
            _message = string.Join(
                            " ",
                            $"{MessageFormatterModel.GetFormattedText("green bold", _assembly.Name)}",
                            $"{MessageFormatterModel.GetFormattedText("white bold", _assembly.Version.ToString())}"
                        );
            _displayer.ShowAppInfo(_message);
        }

        private void ShowDirectorySelector()
        {
            _message = _textResource.GetStringResource("askDirectoryName", new List<string> { MessageFormatterModel.GetFormattedText("springgreen4", "code smells") });
            _directoryName = _displayer.ShowAskMessage(_message);
        }

        private List<string> ShowCodeSmellsSelector() => _displayer.ShowMenu(_analizer.GetOptions());

        private void ShowReportTypeSelector() => _outputExtension = _displayer.ShowOutputExtension();

        private void ShowTotalFilesToProcess()
        {
            _message = _textResource.GetStringResource("notFoundFile");
            if (_filesToProcess.Count > 0)
                _displayer.ShowStatus(TaskStatusModel.INFO, _textResource.GetStringResource("numberOfFile").Replace("{1}", _filesToProcess.Count.ToString())); 
            else 
                _displayer.ShowStatus(TaskStatusModel.WARN, _textResource.GetStringResource("notFoundFile"));
        }

        private void SearchCodeSmells() => _listOfSmells = _displayer.ShowProgress(() => _analizer.SearchSmellsOnDirectory(_filesToProcess, _smellsToAnalyze));
        private void WriteReport()
        {
            _reportWriter = SetStrategy();
            _reportWriter.WriteReport(_filenameOutput, _listOfSmells);
        }

        private void ShowProcessResult()
        {
            string filenameOutput = MessageFormatterModel.GetFormattedText("italic", _filenameOutput);
            _message = _textResource.GetStringResource("okResult").Replace("{1}", filenameOutput);
            _displayer.ShowStatus(TaskStatusModel.SUCCESS, _message);
        }

        //TODO mejorar la forma en que evaluo a que estrategia debe asignarsele la responsabilidad de generar el reporte
        private IReportWriterStrategy SetStrategy()
        {
            switch (_outputExtension.ToLower())
            {
                case "json":
                    return new JSONReportWriterStrategy();
                case "yaml":
                    return new YAMLReportWriterStrategy();
                case "text plain":
                    return new TextPlainReportWriterStrategy();
                default:
                    return new JSONReportWriterStrategy();
            }
        }
        #endregion
    }
}