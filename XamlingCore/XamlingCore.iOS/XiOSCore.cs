using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MonoTouch.UIKit;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.iOS
{
    public class XiOSCore<TRootVm, TInitialVM, TGlue> : XCore<TRootVm, TInitialVM, TGlue>
        where TRootVm : XRootViewModelBase
        where TInitialVM : XViewModel
        where TGlue : IGlue, new()
    {

        private UIWindow _window;

        public void Init()
        {
            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            var msr = new ManualResetEvent(false);

            Task.Run(async () =>
            {
                await _rootVm.Init();
                msr.Set();
            });

            msr.WaitOne();

            _window.RootViewController = null;
            _window.MakeKeyAndVisible();
        }
    }

    public abstract class XCore<TRootVm, TInitialVM, TGlue> 
        where TRootVm : XRootViewModelBase
        where TInitialVM : XViewModel
        where TGlue : IGlue, new()
    {
        protected IContainer _container;
        protected TRootVm _rootVm;

        protected XCore()
        {
            _init();
        } 

        protected void _init()
            
        {
            var glue = new TGlue();
            _container = glue.Container;
            _rootVm = _container.Resolve<TRootVm>();
        }

        
    }
}