using System.IO;

namespace SmellFinderTool.Core.Services.Implementations.ReportWriterStrategies
{
    public class BaseReportWriterStrategy
    {
        public void WriteData(string filenameOutput, string data) => File.WriteAllText(filenameOutput, data);
    }
}