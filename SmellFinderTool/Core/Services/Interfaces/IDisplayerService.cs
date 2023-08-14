using System;
using System.Collections.Generic;
using SmellFinderTool.Core.Models;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IDisplayerService
    {
         void Init(AssemblyModel assembly);
         string AskDirectoryName();
         void ShowDirectorySelected(string directoryName);
         void ClearScreen();
         List<string> ShowMenu(Dictionary<string, string> options);
         List<SmellReportedModel> ShowProgress(Func<List<SmellReportedModel>> task);
         void ShowException(Exception ex);
         void ShowErrorByNonExistDirectory(string directoryName);
         void Exit();
         void ShowCounterOfFiles(int numberOfFiles);
         void ShowEndOfProcess(string fileNameOutput);
         string ShowOutputExtension(ReportSettings config);
    }
}