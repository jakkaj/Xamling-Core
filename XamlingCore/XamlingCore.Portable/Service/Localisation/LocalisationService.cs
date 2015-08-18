using System.Globalization;
using System.Resources;
using XamlingCore.Portable.Contract.Localisation;

namespace XamlingCore.Portable.Service.Localisation
{
    public class LocalisationService : ILocalisationService
    {
        private readonly ILocalisedResourceReader _resourceReader;

        public LocalisationService(ILocalisedResourceReader resourceReader)
        {
            _resourceReader = resourceReader;
        }

        public string Get(string resourceName)
        {
            var result = _resourceReader.GetResource(resourceName, XLocale.CultureInfo);
            return result ?? resourceName;
        }

        public CultureInfo GetCulture()
        {
            return XLocale.CultureInfo;
        }

        public string GetCultureName()
        {
            return XLocale.CultureInfo.Name;
        }
    }
}
