using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.ViewModels;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.XamarinThings.Content.Common;
using XamlingCore.XamarinThings.Content.Navigation;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.XamarinThings.Content.MasterDetail
{
    public abstract class XMasterDetailViewModel : SectionViewModel
    {
        private readonly IViewResolver _viewResolver;
        private Page _masterContent;
        private Page _detailContent;

        protected XViewModel MasterViewModel;

        protected XViewModel CurrentDetail;

        public Command<XViewModel> NavigateToViewModelCommand { get; set; }

        public Color? NavigationTint { get; set; }

        List<INavigationPackage> _packages = new List<INavigationPackage>(); 

        protected XMasterDetailViewModel(IViewResolver viewResolver)
        {
            _viewResolver = viewResolver;
            NavigateToViewModelCommand = new Command<XViewModel>(_onNavigateToPage);
        }

        protected void AddPackage<T>() where T : XViewModel
        {
            var package = new NavigationPackage<T>(Container);
            _packages.Add(package);
            AddPage(package.ViewModel);
        }

        protected void SetMaster(XViewModel masterPage)
        {
            MasterViewModel = masterPage;

            var selectable = MasterViewModel as ISelectableItem<XViewModel>;
            if (selectable != null)
            {
                selectable.SelectionChanged += selectable_SelectionChanged;
            }
        }

        void selectable_SelectionChanged(object sender, EventArgs e)
        {
            var s = sender as ISelectableItem<XViewModel>;

            if (s != null)
            {
                var currentVm = s.Item;
                ShowNavPage(_packages.FirstOrDefault(_=>_.ViewModel == currentVm));
            }
        }



        protected void Build()
        {
            var v = MasterViewModel as IDataListViewModel<XViewModel>;

            if (v == null)
            {
                throw new ArgumentException("Master view model must implement IItemViewModel<XViewModel> so the root master can pass in items to show");
            }

            v.DataList = new ObservableCollection<XViewModel>(SectionViewModels);

            //Resolves the view and also sets the binding context
            //teh view that is associated with the view model will be used
            var masterAreaView = _viewResolver.Resolve(MasterViewModel);
            MasterContent = masterAreaView;

            var firstPage = _packages.First();

            ShowNavPage(firstPage);


            firstPage.ViewModel.OnActivated();
            MasterViewModel.OnActivated();
        }

        protected async virtual Task<bool> OnShowingPage(XViewModel vm)
        {
            return true;
        }

        protected async void ShowNavPage(INavigationPackage package)
        {
            if (!await OnShowingPage(package.ViewModel))
            {
                return;
            }

            package.Showing();

            DetailContent = package.Page;
            CurrentDetail = package.ViewModel;
        }

        void _onNavigateToPage(XViewModel navigateViewModel)
        {

        }

        public override void Dispose()
        {
            if (MasterViewModel == null) return;

            var selectable = MasterViewModel as ISelectableItem<XViewModel>;
            if (selectable != null)
            {
                selectable.SelectionChanged -= selectable_SelectionChanged;
            }

            base.Dispose();
        }

        public Page MasterContent
        {
            get { return _masterContent; }
            set
            {
                _masterContent = value;
                OnPropertyChanged();
            }
        }

        public Page DetailContent
        {
            get { return _detailContent; }
            set
            {
                _detailContent = value;
                OnPropertyChanged();
            }
        }

        protected interface INavigationPackage
        {
            XViewModel ViewModel { get; set; }
            Page Page { get; set; }
            void Showing();
        }

        protected class NavigationPackage<T> : INavigationPackage where T : XViewModel
        {
            private readonly ILifetimeScope _container;

            public NavigationPackage(ILifetimeScope container)
            {
                _container = container;

                var frameManager = _container.Resolve<IFrameManager>();
                RootFrame = XFrame.CreateRootFrame<XRootFrame>(_container);
                var rootNavigationVm = RootFrame.CreateContentModel<XNavigationPageViewModel>();
                Page = frameManager.Init(RootFrame, rootNavigationVm);
                ViewModel = RootFrame.CreateContentModel<T>();
            }

            public XRootFrame RootFrame { get; set; }

            public XViewModel ViewModel { get; set; }

            public Page Page { get; set; }

            public void Showing()
            {
                if (RootFrame.CurrentContentObject != ViewModel)
                {
                    RootFrame.NavigateTo(ViewModel);
                }
            }
        }
    }
}
