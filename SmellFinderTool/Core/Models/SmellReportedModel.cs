using System.Collections.Generic;

namespace SmellFinderTool.Core.Models
{
    public class SmellReportedModel
    {
        #region Fields
        public string File { get; set; }
        public List<SmellInformationModel> Smells { get; set; }
        #endregion

        #region Constructors
        public SmellReportedModel(string fileName, List<SmellInformationModel> smells)
        {
            File = fileName;
            Smells = smells;
        }
        #endregion
    }

    public class SmellInformationModel
    {
        #region Fields
        public string Name { get; set; }
        public string Information { get; set; }
        public List<string> LinesAffected { get; set; }
        #endregion

        #region Constructors
        public SmellInformationModel(string name, string information, List<string> linesAffected)
        {
            Name = name;
            Information = information;
            LinesAffected = linesAffected ?? new List<string>();
        }
        #endregion
    }
}