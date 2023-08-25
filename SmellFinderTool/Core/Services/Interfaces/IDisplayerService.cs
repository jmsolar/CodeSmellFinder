using System;
using System.Collections.Generic;
using SmellFinderTool.Core.Models;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IDisplayerService
    {
        void Init();
        void ShowAppInfo(string message);
        string ShowAskMessage(string message);
        void ClearScreen();
        List<string> ShowMenu(Dictionary<string, string> options);
        List<SmellReportedModel> ShowProgress(Func<List<SmellReportedModel>> task);
        string ShowOutputExtension();
        bool ShowConfirmMessage(string message);
        void ShowSimpleMessage(string message);
        void ShowException(Exception ex);
        void ShowStatus(TaskStatusModel taskStatus, string message);
    }
}