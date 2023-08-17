using System.Collections.Generic;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IResourceService
    {
        string GetStringResource(string key, List<string> valuesToFormat = null, string cultureName = "en-US");
    }
}