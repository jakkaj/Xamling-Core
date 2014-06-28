using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using XamlingCore.iOS.Navigation;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Container;
using XamlingCore.XamarinThings.Contract;

namespace XamlingCore.iOS
{
    /// <summary>
    /// Boots the app to the initial frame
    /// </summary>
    /// <typeparam name="TRootFrame"></typeparam>
    /// <typeparam name="TRootVM"></typeparam>
    /// <typeparam name="TInitialVM"></typeparam>
    /// <typeparam name="TGlue"></typeparam>
    public class XiOSCore<TRootFrame, TRootVM, TGlue> : XCore<TRootFrame, TGlue>
        where TRootFrame : XFrame
        where TRootVM : XViewModel
        where TGlue : class, IGlue, new()
    {
        private IFrameManager _frameManager;

        private UIWindow _window;

        public void Init()
        {
            InitRoot();

            var rootVm = RootFrame.CreateContentModel<TRootVM>();
            _frameManager = RootFrame.Container.Resolve<IFrameManager>();

            var initalViewController = _frameManager.Init(RootFrame, rootVm);

            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            _window.RootViewController = initalViewController.CreateViewController();

            _window.MakeKeyAndVisible();
        }
    }

    public abstract class XCore<TRootFrame, TGlue>
        where TRootFrame : XFrame
        where TGlue : class, IGlue, new()
    {
        protected IContainer Container;
        protected TRootFrame RootFrame;

        protected async void InitRoot()
        {
            var glue = new TGlue();
            glue.Init();

            Container = glue.Container;

            ContainerHost.Container = Container; //sometimes we need to resolve around the place outside of strucutre. 

            RootFrame = XFrame.CreateRootFrame<TRootFrame>(glue.Container.BeginLifetimeScope());            
        }


    }
}