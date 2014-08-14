using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Autofac;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using XamlingCore.iOS.Navigation;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.Model.Navigation;
using XamlingCore.Portable.Platform.Shared.Core;
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
        private UIViewController _rootView;
        private TRootVM _root;
        public void Init()
        {
            InitRoot();

            _root = RootFrame.CreateContentModel<TRootVM>();
            _frameManager = RootFrame.Container.Resolve<IFrameManager>();

            var initalViewController = _frameManager.Init(RootFrame, _root);

            _window = new UIWindow(UIScreen.MainScreen.Bounds);

            _rootView = initalViewController.CreateViewController();
            _window.RootViewController = _rootView;

            _window.MakeKeyAndVisible();
        }

        public override void ShowNativeView(string viewName)
        {
            if (viewName.ToLower().IndexOf("storyboard") != -1)
            {
                _root.Dispatcher.Invoke(() =>
                {
                    var sb = UIStoryboard.FromName(viewName, null);
                    var controller = sb.InstantiateInitialViewController() as UIViewController;
                    _rootView.PresentViewController(controller, false, null);
                });
                return;
            }


            //must be in the main assembly
            var a = Assembly.GetEntryAssembly();

            var t = a.GetType(viewName);

            //is it regsitered?

            if (!Container.IsRegistered(t))
            {
                throw new Exception("Could not find native view: " + t.FullName);
            }

            _root.Dispatcher.Invoke(() =>
            {
                var controller = Container.Resolve(t) as UIViewController;

                if (controller == null)
                {
                    throw new Exception("Could not resolve navtive view as UIViewController: " + t.FullName);
                }
                
                _rootView.PresentViewController(controller, false, null);
            });



        }
    }

}