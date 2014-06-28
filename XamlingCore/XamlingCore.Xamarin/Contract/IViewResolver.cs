using Xamarin.Forms;

namespace XamlingCore.XamarinThings.Contract
{
    public interface IViewResolver
    {
        Page Resolve(object content);
        View ResolveView(object content);
    }
}