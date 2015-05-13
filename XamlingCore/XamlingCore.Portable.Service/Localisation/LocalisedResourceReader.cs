using System.Globalization;
using System.Resources;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.Model.Localisation;

namespace XamlingCore.Portable.Service.Localisation
{
    public class LocalisedResourceReader : ILocalisedResourceReader
    {
        public string GetResource(string name, CultureInfo culture = null)
        {
            return XLocale.ResourceManager.GetString(name, culture);
        }
    }
}
