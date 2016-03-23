using Windows.UI.Xaml;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.UWP.Core;

namespace XamlingCore.UWP.Contract
{
    public interface IXUWPFrameManager
    {
        UIElement Init(XFrame rootFrame, XViewModel rootViewModel, bool isRoot);
        XUWPFrameNavigator FrameNavigator { get; set; }
        XViewModel RootViewModel { get; }
    }
}