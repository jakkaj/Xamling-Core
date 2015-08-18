using System.Globalization;
using System.Resources;

namespace XamlingCore.Portable.Service.Localisation
{
    public static class XLocale
    {
        public static CultureInfo CultureInfo { get; set; }
        public static ResourceManager ResourceManager { get; set; }
    }
}
