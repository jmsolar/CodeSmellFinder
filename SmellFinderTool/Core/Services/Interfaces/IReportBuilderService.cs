using System.Collections.Generic;
using SmellFinder.Models;
using SmellFinderTool.Core.Models;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IReportBuilderService
    {
         void AddSmells(List<SmellResponse> smellsToAdd);
         void AddHeader(string filePath);
         SmellReportedModel GetSmellsDetected();
    }
}