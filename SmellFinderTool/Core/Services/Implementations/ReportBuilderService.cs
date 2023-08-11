using System.Collections.Generic;
using System.Linq;
using SmellFinder.Models;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class ReportBuilderService : IReportBuilderService
    {
        #region Fields
        private SmellReportedModel _smellReported { get; set; }
        #endregion

        #region Public methods
        public void AddHeader(string filePath)
        {
            _smellReported = new SmellReportedModel(
                fileName: filePath,
                smells: new List<SmellInformationModel>()
            );
        }

        public void AddSmells(List<SmellResponse> smellsToAdd)
        {
            foreach (var smell in smellsToAdd)
            {
                var smellReported = new SmellInformationModel(
                    name: smell.Description,
                    information: smell.Message,
                    linesAffected: smell.LinesAffected
                );

                _smellReported.Smells.Add(smellReported);
            }
        }

        public void Reset() => _smellReported = null;

        public SmellReportedModel GetSmellsDetected() => _smellReported;
 
        public bool HasSmells() => _smellReported != null && _smellReported.Smells != null && _smellReported.Smells.Any();
        #endregion
    }
}