using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class ResourceService : IResourceService
    {
        #region Fields
        private ResourceManager _resourceManager;
        public CultureInfo CultureInfo;
        #endregion

        #region Constructors
        public ResourceService()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string baseResourceName = "SmellFinderTool.Resources.Strings";
            _resourceManager = new ResourceManager(baseResourceName, assembly);
            CultureInfo = new CultureInfo("en-US");
        }
        #endregion

        #region Methods
        public string GetStringResource(string key, List<string> valuesToFormat = null, string cultureName = "en-US")
        {
            if (!string.IsNullOrEmpty(cultureName)) CultureInfo = new CultureInfo(cultureName);

            string localizedText = _resourceManager.GetString(key, CultureInfo);
            localizedText = string.Format(localizedText, valuesToFormat.ToArray());

            return localizedText;
        }
        #endregion
    }
}