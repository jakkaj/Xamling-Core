using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Localisation;
using XamlingCore.Portable.DTO.Localisation;

namespace XamlingCore.Portable.Localisation
{
    public static class LocalisableHelper
    {
        public static string GetResource(IXLocalisable localisable, string name, CultureInfo culture)
        {
            if (localisable == null || 
                localisable.XLocalisedResources == null || 
                localisable.XLocalisedResources.Resources == null || 
                localisable.XLocalisedResources.Resources.Count == 0)
            {
                return null;
            }

            var cultureName = culture.Name;
            var cultureSuperName = culture.TwoLetterISOLanguageName;

            const string defaultCultureName = "en";

            var resources = localisable.XLocalisedResources.Resources;

            var i = resources.FirstOrDefault(_ => _.CultureName == cultureName) ??
                    resources.FirstOrDefault(_ => _.CultureName == cultureSuperName) ??
                    resources.FirstOrDefault(_ => _.CultureName == defaultCultureName);

            if (i == null)
            {
                return null;
            }

            var val = i.Values.FirstOrDefault(_ => _.Name == name);
            return val != null ? _ecodeEncodedNonAsciiCharacters(val.Value) : null;
        }

        public static string GetResource(XLocalisedResources localisedResources, string name, CultureInfo culture)
        {
            var cultureName = culture.Name;
            var cultureSuperName = culture.TwoLetterISOLanguageName;
            var resources = localisedResources.Resources;

            var i = resources.FirstOrDefault(_ => _.CultureName == cultureName) ??
                    resources.FirstOrDefault(_ => _.CultureName == cultureSuperName);

            if (i == null)
            {
                return null;
            }

            var val = i.Values.FirstOrDefault(_ => _.Name == name);

            return val != null ? _ecodeEncodedNonAsciiCharacters(val.Value) : null;
        }


        static string _ecodeEncodedNonAsciiCharacters(string value)
        {
            if (value == null)
            {
                return null;
            }

            var result = Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());

            if (result != null)
            {
                result = result.Replace("\\r\\n", "\r\n");
            }

            return result;
        }
    }
}
