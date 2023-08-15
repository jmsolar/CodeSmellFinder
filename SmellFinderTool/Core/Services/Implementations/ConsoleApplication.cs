using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Implementations.ReportWriterStrategies;
using SmellFinderTool.Core.Services.Interfaces;

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
        private ReportSettings _config;
        #endregion

        #region Constructors
        public ConsoleApplication(
            IDisplayerService displayer,
            IAnalizerService analizer,
            IFileSystemManagerService fileSystemManager,
            ReportSettings config
        )
        {
            _displayer = displayer;
            _analizer = analizer;
            _fileSystemManager = fileSystemManager;
            _config = config;

            _assembly = new AssemblyModel(
                    name: Assembly.GetExecutingAssembly().GetName().Name,
                    version: Assembly.GetExecutingAssembly().GetName().Version
            );
        }
        #endregion

        #region Methods
        public async Task Run()
        {
            var follow = true;
            string directoryName = string.Empty;

            while (follow)
            {
                try
                {
                    do
                    {
                        _displayer.Init(_assembly);
                        directoryName = _displayer.ShowAskMessage($"\n\nWhich directory are you going to look for {MessageFormatterModel.GetFormattedText("springgreen4", "code smells")}?");

                        var filesToProcess = await _fileSystemManager.GetFilesToProcess(directoryName);
                        var optionsSelected = _displayer.ShowMenu(_analizer.GetOptions());

                        if (optionsSelected.Any())
                        {
                            _displayer.ShowDirectorySelected(directoryName);
                            var outputExtension = _displayer.ShowOutputExtension(_config);
                            _displayer.ShowCounterOfFiles(filesToProcess.Count);

                            var filenameOutput = _fileSystemManager.GetFileNameOutput(directoryName, outputExtension);

                            var action = new Action(() => _analizer.SearchSmellsOnDirectory(filesToProcess, optionsSelected));
                            var res = _displayer.ShowProgress(() =>
                            {
                                return _analizer.SearchSmellsOnDirectory(filesToProcess, optionsSelected);
                            });

                            _reportWriter = SetStrategy(outputExtension);
                            _reportWriter.WriteReport(filenameOutput, res);
                            _displayer.ShowEndOfProcess(filenameOutput);
                        }

                        follow = _displayer.ShowConfirmMessage("Want you search others smells?");

                        if (follow) _displayer.ClearScreen();
                    } while (follow);
                }
                catch (Exception ex)
                {
                    _displayer.ShowSimpleMessage(ex.Message);
                    follow = _displayer.ShowConfirmMessage("Want you search others smells?");
                }
            }   
        }
        #endregion

        #region Private methods
        private IReportWriterStrategy SetStrategy(string outputExtension)
        {
            switch (outputExtension.ToLower())
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