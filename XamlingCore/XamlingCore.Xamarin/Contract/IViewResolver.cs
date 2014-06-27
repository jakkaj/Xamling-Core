using Xamarin.Forms;

namespace XamlingCore.Xamarin.Contract
{
    public interface IViewResolver
    {
        Page Resolve(object content);
    }
}