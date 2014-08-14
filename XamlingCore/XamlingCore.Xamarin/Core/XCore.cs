using Autofac;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Container;

namespace XamlingCore.XamarinThings.Core
{
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

            XMessenger.Default.Register<ShowNativeViewMessage>(this, _onShowNativeView);
        }

        void _onShowNativeView(object m)
        {
            var message = m as ShowNativeViewMessage;

            if (message == null)
            {
                return;
            }

            ShowNativeView(message.ViewName);
        }

        public abstract void ShowNativeView(string viewName);
    }
}
