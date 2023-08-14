using System.Collections.Generic;
using System.Text.Json;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations.ReportWriterStrategies
{
    public class JSONReportWriterStrategy : BaseReportWriterStrategy, IReportWriterStrategy
    {
        #region Methods
        public void WriteReport(string filenameOutput, List<SmellReportedModel> data)
        {
            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = false,
                IgnoreNullValues = true,
                AllowTrailingCommas = false
            });

            WriteData(
                filenameOutput,
                jsonString
            );
        }
        #endregion
    }
}