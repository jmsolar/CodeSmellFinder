using System.Collections.Generic;

namespace SmellFinderTool.Core.Models
{
    public class ReportSettings
    {
        public List<FileExtension> FileExtension { get; set; }
    }

    public class FileExtension
    {
        public string Type { get; set; }
        public string Description { get; set; }
    }
}