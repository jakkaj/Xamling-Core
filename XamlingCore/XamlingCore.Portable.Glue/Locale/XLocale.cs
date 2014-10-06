using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Glue.Locale
{
    public static class XLocale
    {
        public static CultureInfo CultureInfo { get; set; }
        public static ResourceManager ResourceManager { get; set; }
    }
}
