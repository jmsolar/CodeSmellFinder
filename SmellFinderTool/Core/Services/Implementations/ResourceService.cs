using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class ResourceService : IResourceService
    {
        #region Fields
        private CultureInfo CultureInfo;
        private ResourceManager _resourceManager;
        private readonly string defaultISOCode = "en-us"; 
        private readonly HashSet<string> ValidLanguageCodes = new HashSet<string>(CultureInfo.GetCultures(CultureTypes.AllCultures).Select(culture => culture.Name.ToLower()));
        #endregion

        #region Constructors
        public ResourceService()
        {
            string baseResourceName = "SmellFinderTool.Resources.Strings";
            _resourceManager = new ResourceManager(baseResourceName, Assembly.GetExecutingAssembly());
        }
        #endregion

        #region Methods
        public void SetLanguage(string ISOCode) {
            try
            {
                ISOCode = ISOCode?.ToLower();
                string lang = IsLanguageValid(ISOCode) ? ISOCode : defaultISOCode;
                CultureInfo = new CultureInfo(lang);   
            }
            catch (Exception)
            {
                CultureInfo = new CultureInfo(defaultISOCode);
            }
        }

        private bool IsLanguageValid(string ISOCode) => ValidLanguageCodes.Contains(ISOCode);

        public string GetStringResource(string key, List<string> valuesToFormat = null)
        {
            try
            {
                string localizedText = _resourceManager.GetString(key, CultureInfo);
                localizedText = valuesToFormat == null ? localizedText : string.Format(localizedText, valuesToFormat.ToArray());

                return localizedText;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}