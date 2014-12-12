using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.Services
{
    public interface ILoadStatusService
    {
        Task Loader(Task awaiter, string text = null);
        Task<T> Loader<T>(Task<T> awaiter, string text = null);
        void ShowIndicator(string text);
        void HideIndicator();
    }
}