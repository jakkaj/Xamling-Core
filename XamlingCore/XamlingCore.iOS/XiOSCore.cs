using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.iOS.Navigation;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.iOS
{
    /// <summary>
    /// Boots the app to the initial frame
    /// </summary>
    /// <typeparam name="TRootFrame"></typeparam>
    /// <typeparam name="TRootVM"></typeparam>
    /// <typeparam name="TInitialVM"></typeparam>
    /// <typeparam name="TGlue"></typeparam>
    public class XiOSCore<TRootFrame,TRootVM, TInitialVM, TGlue> : XCore<TRootFrame, TRootVM, TInitialVM, TGlue>
        where TRootFrame : XFrame
        where TRootVM : XViewModel
        where TInitialVM : XViewModel
        where TGlue : class, IGlue, new()
    {

        private iOSFrameManager _frameManager;

        private UIWindow _window;

        public XiOSCore()
        {
            
        }

        public void Init()
        {
            InitRoot();

            var rootVm = RootFrame.CreateContentModel<TRootVM>();
            var initialVm = rootVm.CreateContentModel<TInitialVM>();

            _frameManager = RootFrame.Container.Resolve<iOSFrameManager>();

            var initalViewController = _frameManager.Init(RootFrame, rootVm, initialVm);

            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            _window.RootViewController = initalViewController;

            _window.MakeKeyAndVisible();
        }
    }

    public abstract class XCore<TRootFrame, TRootVM, TInitialVM, TGlue>
        where TRootFrame : XFrame
        where TRootVM : XViewModel
        where TInitialVM : XViewModel
        where TGlue : class, IGlue, new()
    {
        protected IContainer Container;
        protected TRootFrame RootFrame;

        protected async void InitRoot()
        {
            var glue = new TGlue();
            glue.Init();

            Container = glue.Container;
            RootFrame = XFrame.CreateRootFrame<TRootFrame>(glue.Container.BeginLifetimeScope());

            await RootFrame.Init();

            var initalVm = RootFrame.CreateContentModel<TInitialVM>();

            if (initalVm == null)
            {
                throw new Exception("Initial VM could not be resolved, ensure viewmodels are registered");
            }

            RootFrame.NavigateTo(initalVm);
        }


    }
}