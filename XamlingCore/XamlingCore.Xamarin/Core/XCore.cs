using System.Threading.Tasks;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.XamarinThings.Core
{
    public class XCore<TRootViewModel, TGlue>
        
        where TRootViewModel : XViewModel
        where TGlue : class, IGlue, new()
    {
        protected IContainer Container;
        protected XRootFrame RootFrame;

        private IFrameManager _frameManager;
        
        public Page InitRoot()
        {
            var glue = new TGlue();
            glue.Init();

            Container = glue.Container;

            ContainerHost.Container = Container; //sometimes we need to resolve around the place outside of strucutre. 

            RootFrame = XFrame.CreateRootFrame<XRootFrame>(glue.Container.BeginLifetimeScope());

            var rootPage = GetRootPage<TRootViewModel>();

            return rootPage;
        }

        public Page GetRootPage<TViewModel>() where TViewModel : XViewModel
        {
            _frameManager = RootFrame.Container.Resolve<IFrameManager>();
            var newRoot = RootFrame.CreateContentModel<TViewModel>();

            XFrameManager.AlertHandler = null;
            var initalViewController = _frameManager.Init(RootFrame, newRoot);
            var p = initalViewController;
            return p;
        }
    }
}
