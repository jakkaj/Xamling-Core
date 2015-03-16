using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Core;

namespace XamlingCore.Droid
{
    public class XDriodCore<TRootFrame, TRootVM, TGlue> : XCore<TRootFrame, TGlue>
        where TRootFrame : XFrame
        where TRootVM : XViewModel
        where TGlue : class, IGlue, new()
    {

        private IFrameManager _frameManager;

        private TRootVM _root;

        private Page _rootPage;

        public TRootVM RootViewModel
        {
            get { return _root; }
        }

        public Xamarin.Forms.Application Init()
        {
            InitRoot();
            XCorePlatform.Platform = XCorePlatform.XCorePlatforms.Android;

            _root = RootFrame.CreateContentModel<TRootVM>();
            _frameManager = RootFrame.Container.Resolve<IFrameManager>();

            var initalViewController = _frameManager.Init(RootFrame, RootViewModel);

            _rootPage = initalViewController;

            var app = new XApp();

            app.SetMainPage(_rootPage);


            return app;
        }

        public override void ShowNativeView(string viewName)
        {
            throw new NotImplementedException();
        }
    }
}