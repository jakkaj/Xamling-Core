using Autofac;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Data.Glue;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.UWP.Contract;

namespace XamlingCore.UWP.Core
{
    public class XUWPCore<TRootViewModel, TGlue>

        where TRootViewModel : XViewModel
        where TGlue : class, IGlue, new()
    {
        protected IContainer Container;
        protected XUWPRootFrame RootFrame;

        private IXUWPFrameManager _frameManager;

        public void InitRoot()
        {
            var glue = new TGlue();
            glue.Init();

            Container = glue.Container;

            ContainerHost.Container = Container; //sometimes we need to resolve around the place outside of strucutre. 

            RootFrame = XFrame.CreateRootFrame<XUWPRootFrame>(glue.Container.BeginLifetimeScope());

            _init<TRootViewModel>();
        }

        public void _init<TViewModel>() where TViewModel : XViewModel
        {
            _frameManager = RootFrame.Container.Resolve<IXUWPFrameManager>();
            var newRoot = RootFrame.CreateContentModel<TViewModel>();
           
            _frameManager.Init(RootFrame, newRoot, true);

            
        }

        public IXUWPFrameManager FrameManager
        {
            get { return _frameManager; }
        }
    }
}
