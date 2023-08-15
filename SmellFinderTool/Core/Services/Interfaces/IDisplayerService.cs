using System;
using System.Collections.Generic;
using SmellFinderTool.Core.Models;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IDisplayerService
    {
         void Init(AssemblyModel assembly);
         string ShowAskMessage(string message);
         void ShowDirectorySelected(string directoryName);
         void ClearScreen();
         List<string> ShowMenu(Dictionary<string, string> options);
         List<SmellReportedModel> ShowProgress(Func<List<SmellReportedModel>> task);
         void ShowCounterOfFiles(int numberOfFiles);
         void ShowEndOfProcess(string fileNameOutput);
         string ShowOutputExtension(ReportSettings config);
         bool ShowConfirmMessage(string message);
         void ShowSimpleMessage(string message);
    }
}