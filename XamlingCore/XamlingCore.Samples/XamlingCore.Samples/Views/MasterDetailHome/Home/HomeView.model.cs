using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.Views.MasterDetailHome.Home.DynamicContentExamples;
using XamlingCore.Samples.Views.MasterDetailHome.Tabs;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class HomeViewModel : XViewModel
    {
        private readonly IEntityCache _cache;
        public ICommand NextPageCommand { get; set; }
        public ICommand ShowNativeViewCommand { get; set; }

        private XViewModel _dynamicViewModel;

        public HomeViewModel(IEntityCache cache)
        {
            _cache = cache;
            Title = "Home";
            NextPageCommand = new Command(_nextPage);
            ShowNativeViewCommand = new Command(_onShowNativeView);
        }

        public override void OnInitialise()
        {
            _doThings();

            base.OnInitialise();
        }

        async void _doThings()
        {
           
            int count = 0;

            await Task.Delay(4000);

            DynamicViewModel = CreateContentModel<FirstDynamicViewModel>();

            while (count < 3)
            {
                await Task.Delay(3000);
                if (DynamicViewModel is FirstDynamicViewModel)
                {
                    DynamicViewModel = CreateContentModel<SecondDynamicViewModel>();
                }
                else
                {
                    DynamicViewModel = CreateContentModel<FirstDynamicViewModel>();
                }
                count ++;

                
            }

            DynamicViewModel = null;


             count = 0;

            while (count < 3)
            {
                await Task.Delay(3000);
                if (DynamicViewModel is FirstDynamicViewModel)
                {
                    DynamicViewModel = CreateContentModel<SecondDynamicViewModel>();
                }
                else
                {
                    DynamicViewModel = CreateContentModel<FirstDynamicViewModel>();
                }
                count++;


            }

            DynamicViewModel = null;


        }

        void _onShowNativeView()
        {
            //new ShowNativeViewMessage("XamlingCore.Samples.iOS.NativeViews.SomeNativeView").Send();
            new ShowNativeViewMessage("NativeStoryboardView").Send();
        }

        void _nextPage()
        { 
            //NavigateTo<HomeTabsViewModel>();
            NavigateTo<HomePageTwoViewModel>();
        }

        public XViewModel DynamicViewModel
        {
            get { return _dynamicViewModel; }
            set
            {
                _dynamicViewModel = value;
                OnPropertyChanged();
            }
        }

        
    }
}
