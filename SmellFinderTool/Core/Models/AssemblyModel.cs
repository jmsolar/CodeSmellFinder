using System;

namespace SmellFinderTool.Core.Models
{
    public class AssemblyModel
    {
        #region Fields
        public string Name { get; set; }
        public Version Version { get; set; }
        #endregion

        #region Constructors
        public AssemblyModel(string name, Version version)
        {
            Name = name;
            Version = version;
        }
        #endregion
    }
}