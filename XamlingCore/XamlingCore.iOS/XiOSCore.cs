using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamlingCore.iOS.Navigation;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.iOS
{
    public class XiOSCore<TRootFrame,TRootVM, TInitialVM, TGlue> : XCore<TRootFrame, TRootVM, TInitialVM, TGlue>
        where TRootFrame : XFrame
        where TRootVM : XViewModel
        where TInitialVM : XViewModel
        where TGlue : class, IGlue, new()
    {

        private iOSNavigator _navigator;

        public void Init()
        {
            InitRoot();
            _navigator = new iOSNavigator(RootFrame, RootFrame.Navigation, RootFrame.Container);
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