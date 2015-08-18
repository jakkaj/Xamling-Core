using System.Globalization;

namespace XamlingCore.Portable.Contract.Localisation
{
    public interface ILocalisationService
    {
        string Get(string resourceName);
        string GetCultureName();
        CultureInfo GetCulture();
    }
}