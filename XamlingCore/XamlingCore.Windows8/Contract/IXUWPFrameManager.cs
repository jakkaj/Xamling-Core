using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Windows8.Core;

namespace XamlingCore.Windows8.Contract
{
    public interface IXUWPFrameManager
    {
        void Init(XFrame rootFrame, XViewModel rootViewModel);
        XUWPFrameNavigator FrameNavigator { get; set; }
        XViewModel RootViewModel { get; }
    }
}