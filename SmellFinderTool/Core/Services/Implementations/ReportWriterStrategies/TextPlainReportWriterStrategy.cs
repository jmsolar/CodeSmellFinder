using System.Collections.Generic;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations.ReportWriterStrategies
{
    public class TextPlainReportWriterStrategy : BaseReportWriterStrategy, IReportWriterStrategy
    {
        #region Methods
        public void WriteReport(string filenameOutput, List<SmellReportedModel> data)
        {
            string reportText = string.Empty;
            foreach (var item in data)
            {
                reportText = $"{item.File}\n";

                foreach (var smell in item.Smells)
                {
                    reportText += $"{smell.Name}: {smell.Information}\n";
                    reportText += $"Lines Affected: {string.Join(", ", smell.LinesAffected)}\n";
                }
            }

            WriteData(
                filenameOutput,
                reportText
            );
        }
        #endregion
    }
}