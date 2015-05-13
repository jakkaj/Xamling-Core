using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.View.Properties;

namespace XamlingCore.Windows8.Implementations
{
    public class LocalisedResourceReader : ILocalisedResourceReader
    {
        private readonly ResourceLoader _resourceLoader;
        public LocalisedResourceReader()
        {
            _resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
        }
        public string GetResource([NotNull] string name, CultureInfo culture = null)
        {
            return _resourceLoader.GetString(name);
        }
    }
}
