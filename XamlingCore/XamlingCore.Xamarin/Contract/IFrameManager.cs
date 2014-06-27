using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.XamarinThings.Contract
{
    public interface IFrameManager
    {
        Page Init(XFrame rootFrame, XViewModel rootViewModel);
        IFrameNavigator FrameNavigator { get; set; }
        XViewModel RootViewModel { get; set; }
    }
}