using System.Collections.Generic;
using SmellFinderTool.Core.Models;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IAnalizerService
    {
        Dictionary<string, string> GetOptions();
        void SearchSmellsOnDirectory(List<string> filesToProcess, List<string> smellsSelected);
        List<SmellReportedModel> GetSmellsDetected();
    }
}