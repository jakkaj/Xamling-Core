using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Windows8.Contract;
using XamlingCore.Windows8.View;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.Windows8.Core
{
    public class XUWPCore<TRootViewModel, TGlue>

        where TRootViewModel : XViewModel
        where TGlue : class, IGlue, new()
    {
        protected IContainer Container;
        protected XRootFrame RootFrame;

        private IXUWPFrameManager _frameManager;

        public void InitRoot()
        {
            var glue = new TGlue();
            glue.Init();

            Container = glue.Container;

            ContainerHost.Container = Container; //sometimes we need to resolve around the place outside of strucutre. 

            RootFrame = XFrame.CreateRootFrame<XRootFrame>(glue.Container.BeginLifetimeScope());

            _init<TRootViewModel>();
        }

        public void _init<TViewModel>() where TViewModel : XViewModel
        {
            _frameManager = RootFrame.Container.Resolve<IXUWPFrameManager>();
            var newRoot = RootFrame.CreateContentModel<TViewModel>();
           
            _frameManager.Init(RootFrame, newRoot);

            
        }

        public IXUWPFrameManager FrameManager
        {
            get { return _frameManager; }
        }
    }
}
