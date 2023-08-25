using System.Collections.Generic;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IResourceService
    {
        void SetLanguage(string ISOCode);
        string GetStringResource(string key, List<string> valuesToFormat = null);
    }
}