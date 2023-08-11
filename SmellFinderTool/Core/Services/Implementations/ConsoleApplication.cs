using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SmellFinderTool.Core.Models;
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
        #endregion

        #region Constructors
        public ConsoleApplication(
            IDisplayerService displayer,
            IAnalizerService analizer,
            IFileSystemManagerService fileSystemManager
        )
        {
            _displayer = displayer;
            _analizer = analizer;
            _fileSystemManager = fileSystemManager;

            _assembly = new AssemblyModel(
                    name: Assembly.GetExecutingAssembly().GetName().Name,
                    version: Assembly.GetExecutingAssembly().GetName().Version
            );
        }
        #endregion

        #region Methods
        public async Task Run()
        {
            try
            {
                _displayer.Init(_assembly);

                var directoryName = _displayer.AskDirectoryName();
                var existPath = _fileSystemManager.IsValidDirectory(directoryName);

                if (existPath)
                {
                    var filesToProcess = await _fileSystemManager.GetFilesToProcess(directoryName);
                    var optionNames = _analizer.GetOptions();
                    var optionsSelected = _displayer.ShowMenu(optionNames);

                    if (optionsSelected.Any()) {
                        _displayer.ShowDirectorySelected(directoryName);
                        _displayer.ShowCounterOfFiles(filesToProcess.Count);

                        var fileNameOutput = _fileSystemManager.GetFileNameOutput(directoryName, "json");
                        var tasks = new Action[]
                        {
                            () => _analizer.SearchSmellsOnDirectory(filesToProcess, optionsSelected),
                            () => GenerateReport(fileNameOutput)
                        };

                        _displayer.ShowProgress(tasks);
                        _displayer.ShowEndOfProcess(fileNameOutput);
                    }
                }
                else _displayer.ShowErrorByNonExistDirectory(directoryName);
            }
            catch (Exception ex)
            {
                _displayer.ShowException(ex);
            }
            finally
            {
                _displayer.Exit();
            }
        }
        #endregion

        #region Private methods
        private void GenerateReport(string fileNameOutput) => _fileSystemManager.AddReportData(fileNameOutput, _analizer.GetSmellsDetected());
        #endregion
    }
}