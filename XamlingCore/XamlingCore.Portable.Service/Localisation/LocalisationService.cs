using System.Globalization;
using System.Resources;
using XamlingCore.Portable.Contract.Localisation;

namespace XamlingCore.Portable.Service.Localisation
{
    public class LocalisationService : ILocalisationService
    {
        private readonly CultureInfo _resourceCulture;
        private readonly ResourceManager _resourceManager;

        public LocalisationService(CultureInfo resourceCulture, ResourceManager resourceManager)
        {
            _resourceCulture = resourceCulture;
            _resourceManager = resourceManager;
        }

        public string Get(string resourceName)
        {
            var result =  _resourceManager.GetString(resourceName, _resourceCulture);
            return result ?? resourceName;
        }

        public CultureInfo GetCulture()
        {
            return _resourceCulture;
        }

        public string GetCultureName()
        {
            return _resourceCulture.Name;
        }
    }
}
