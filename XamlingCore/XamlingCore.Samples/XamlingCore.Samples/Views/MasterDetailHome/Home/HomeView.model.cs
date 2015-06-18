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
using XamlingCore.Portable.Contract.Infrastructure.LocalStorage;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.Views.MasterDetailHome.Home.DynamicContentExamples;
using XamlingCore.Samples.Views.MasterDetailHome.List;
using XamlingCore.Samples.Views.MasterDetailHome.Tabs;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class HomeViewModel : XViewModel
    {
        private readonly IEntityCache _cache;
        private readonly ILocalStorage _storage;
        public ICommand NextPageCommand { get; set; }
        public ICommand ShowNativeViewCommand { get; set; }

        private XViewModel _dynamicViewModel;

        public ICommand RepeatersCommand { get; set; }

        public HomeViewModel(IEntityCache cache, ILocalStorage storage)
        {
            _cache = cache;
            _storage = storage;
            Title = "Home";
            NextPageCommand = new Command(_nextPage);
            ShowNativeViewCommand = new Command(_onShowNativeView);

            RepeatersCommand = new Command(_onRepeaters);
        }

        private void _onRepeaters()
        {
            NavigateTo<RepeaterListViewModel>();
        }

        public override void OnInitialise()
        {
            ParentModel.Navigation.RestrictNavigationTime = TimeSpan.FromSeconds(1);
            _doThings();

            base.OnInitialise();
        }

        async void _doThings()
        {

            var fileName = "something\\someting\\\\SomethuingElse\\tes.dat";

            await _storage.SaveString(fileName, "Testing123");

            var fileData = await _storage.LoadString(fileName);

            var fileExists = await _storage.FileExists(fileName);

            await _storage.DeleteFile(fileName);

            var fileExistsAFter = await _storage.FileExists(fileName);

            int count = 0;

            await Task.Delay(4000);

            DynamicViewModel = CreateContentModel<FirstDynamicViewModel>();

            await Task.Delay(2000);
            DynamicViewModel = null;

            return;


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
            NavigateToModal<HomePageTwoViewModel>();
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
