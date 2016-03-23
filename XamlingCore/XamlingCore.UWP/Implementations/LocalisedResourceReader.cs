using System.Globalization;
using Windows.ApplicationModel.Resources;
using XamlingCore.Portable.Contract.Localisation;

namespace XamlingCore.UWP.Implementations
{
    public class LocalisedResourceReader : ILocalisedResourceReader
    {
        private readonly ResourceLoader _resourceLoader;
        public LocalisedResourceReader()
        {
            _resourceLoader = new ResourceLoader();
        }
        public string GetResource(string name, CultureInfo culture = null)
        {
            if (name == null)
            {
                return null;
            }

            return _resourceLoader.GetString(name);
        }
    }
}
