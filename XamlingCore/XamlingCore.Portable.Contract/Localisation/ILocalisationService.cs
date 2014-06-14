using System.Globalization;

namespace XamlingCore.Portable.Contract.Localisation
{
    public interface ILocalisationService
    {
        string GetLocalisedResource(string resourceName);
        string GetCultureName();
        CultureInfo GetCulture();
    }
}