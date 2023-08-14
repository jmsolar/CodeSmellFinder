using System.Collections.Generic;
using SmellFinderTool.Core.Models;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IReportWriterStrategy
    {
         void WriteReport(string filenameOutput, List<SmellReportedModel> data);
    }
}