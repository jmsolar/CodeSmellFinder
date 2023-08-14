using System.Collections.Generic;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Interfaces;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SmellFinderTool.Core.Services.Implementations.ReportWriterStrategies
{
    public class YAMLReportWriterStrategy : BaseReportWriterStrategy, IReportWriterStrategy
    {
        #region Methods
        public void WriteReport(string filenameOutput, List<SmellReportedModel> data)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string yaml = serializer.Serialize(data);

            WriteData(
                filenameOutput,
                yaml
            );
        }
        #endregion
    }
}