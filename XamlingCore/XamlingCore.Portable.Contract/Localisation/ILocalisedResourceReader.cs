using System.Globalization;

namespace XamlingCore.Portable.Contract.Localisation
{
    public interface ILocalisedResourceReader
    {
        string GetResource(string name, CultureInfo culture = null);
    }
}