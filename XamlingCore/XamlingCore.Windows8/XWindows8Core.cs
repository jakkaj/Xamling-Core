using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Autofac;
using Autofac.Core;
using XamlingCore.Portable.Contract.Glue;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Core;

namespace XamlingCore.Windows8
{
    public class XWindows8Core<TRootFrame, TRootVM, TGlue> : XCore<TRootFrame, TGlue>
        where TRootFrame : XFrame
        where TRootVM : XViewModel
        where TGlue : class, IGlue, new()
    {

        private TRootVM _rootViewModel;
        private Frame _rootVisualFrame;

        public void Init()
        {
            InitRoot();

            RootFrame.Navigation.Navigated += Navigation_Navigated;

            XCorePlatform.Platform = XCorePlatform.XCorePlatforms.Windows8;
            _rootViewModel = RootFrame.CreateContentModel<TRootVM>();
        }

        void Navigation_Navigated(object sender, XamlingCore.Portable.Model.Navigation.XNavigationEventArgs e)
        {
            _doNavigate();   
        }

        void _doNavigate()
        {
            var cco = RootFrame.Navigation.CurrentContentObject;
            if (cco != null)
            {
                var t = cco.GetType();

                if (t.Name.Contains("ViewModel"))
                {
                    var typeName = t.AssemblyQualifiedName.Replace("ViewModel", "View");
                    var viewType = Type.GetType(typeName);
                    _rootVisualFrame.Navigate(viewType, cco);
                }
            }
        }

        public void SetRootFrame(Frame frame)
        {
            _rootVisualFrame = frame;
            RootFrame.NavigateTo(_rootViewModel);
        }

        public override void ShowNativeView(string viewName)
        {
            throw new NotImplementedException();
        }
    }
}
