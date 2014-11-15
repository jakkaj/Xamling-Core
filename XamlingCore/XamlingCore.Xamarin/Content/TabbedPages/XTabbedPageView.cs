using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Content.Common;
using XamlingCore.XamarinThings.Content.Dynamic;
using XamlingCore.XamarinThings.Content.MasterDetail;

namespace XamlingCore.XamarinThings.Content.TabbedPages
{
    public class XTabbedPageView : TabbedPage
    {
        private readonly IOrientationSensor _orientationSensor;
        private XTabbedPageViewModel _viewModel;

        public XTabbedPageView(IOrientationSensor orientationSensor)
        {
            _orientationSensor = orientationSensor;
            ItemTemplate = new DataTemplate(typeof(DynamicContentPage));
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            _orientationSensor.OnRotated();
            base.OnSizeAllocated(width, height);
        }

        protected override void OnBindingContextChanged()
        {
            _viewModel = BindingContext as XTabbedPageViewModel;

            if (_viewModel == null)
            {
                throw new ArgumentException("BindingContext must be XTabbedPageViewModel");
            }

            ItemsSource = _viewModel.SectionViewModels;

            base.OnBindingContextChanged();
        }

        protected override void OnCurrentPageChanged()
        {
            var page = CurrentPage;

            if (page == null)
            {
                return;
            }

            var vm = page.BindingContext as XViewModel;

            if (vm == null)
            {
                return;
            }

            Title = vm.Title;

            var thisvm = BindingContext as SectionViewModel;
            
            if (thisvm != null)
            {
                thisvm.CurrentSection = vm;
            }

            base.OnCurrentPageChanged();
        }
    }
}
