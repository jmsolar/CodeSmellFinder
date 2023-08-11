using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmellFinder.Attributes;
using SmellFinder.Scanners;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class AnalizerService : IAnalizerService
    {
        #region Fields
        private readonly IParserService _parser;
        private readonly IFileSystemManagerService _fileSystemManager;
        private readonly IReportBuilderService _reportBuilder;
        private List<SmellReportedModel> _report { get; set; }
        #endregion

        #region Constructors
        public AnalizerService(IParserService parser, IFileSystemManagerService fileSystemManager, IReportBuilderService reportBuilder)
        {
            _parser = parser;
            _fileSystemManager = fileSystemManager;
            _reportBuilder = reportBuilder;
            _report = new List<SmellReportedModel>();
        }
        #endregion
        
        #region Methods
        public Dictionary<string, string> GetOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            var definedIn = typeof(VisitorAttribute).Assembly.GetName().Name;

            foreach (var type in from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
                                 where (!assembly.GlobalAssemblyCache) && ((assembly.GetName().Name == definedIn) || assembly.GetReferencedAssemblies().Any(a => a.Name == definedIn))
                                 from Type type in assembly.GetTypes()
                                 select type)
            {
                var visitorAttributes = type.GetCustomAttributes(typeof(VisitorAttribute), true);
                if (visitorAttributes.Length > 0)
                {
                    VisitorAttribute attr = (VisitorAttribute)Attribute.GetCustomAttribute(type, typeof(VisitorAttribute));
                    options.TryAdd(attr.Name, attr.Description);
                }
            }

            return options;
        }

        public void SearchSmellsOnDirectory(List<string> filesToProcess, List<string> smells)
        {
            foreach (var filePath in filesToProcess)
            {
                var fileContent = _fileSystemManager.LoadFileContent(filePath);

                if (!string.IsNullOrEmpty(fileContent)) {
                    var jsParser = _parser.GenerateJSParser(fileContent);
                    var smellsFounded = VisitorScanner.Search(smells, jsParser);

                    if (smellsFounded.Any()) {
                        _reportBuilder.AddHeader(filePath);
                        _reportBuilder.AddSmells(smellsFounded);   
                    }
                }

                if (_reportBuilder.HasSmells()) _report.Add(_reportBuilder.GetSmellsDetected());
                _reportBuilder.Reset();
            }
        }

        public List<SmellReportedModel> GetSmellsDetected() => _report;
        #endregion
    }
}