using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using Autofac.Core;
using XamlingCore.Portable.Contract.ViewModels;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.View;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.UWP.Contract;
using XamlingCore.UWP.Core;
using XamlingCore.UWP.View;

namespace XamlingCore.UWP.Navigation.MasterDetail
{
    public abstract class XUWPMasterDetailViewModel : SectionViewModel
    {
        private readonly IUWPViewResolver _viewResolver;
        private UIElement _masterContent;
        private UIElement _detailContent;

        protected XViewModel _masterViewModel;

        protected XViewModel CurrentDetail;

        public XCommand<XViewModel> NavigateToViewModelCommand { get; set; }

        public Color? NavigationTint { get; set; }

        List<INavigationPackage> _packages = new List<INavigationPackage>(); 

        protected XUWPMasterDetailViewModel(IUWPViewResolver viewResolver)
        {
            _viewResolver = viewResolver;
            NavigateToViewModelCommand = new XCommand<XViewModel>(_onNavigateToPage);
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

            CurrentDetail?.OnDeactivated();

            package.Showing();

            DetailContent = package.Page;
            CurrentDetail = package.ViewModel;

            CurrentDetail?.OnActivated();
        }

        public void ShowViewModel(XViewModel vm)
        {
            var vmPage = _packages.FirstOrDefault(_ => _.ViewModel == vm);

            if (vmPage == null)
            {
                return;
            }

            ShowNavPage(vmPage);
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

        public UIElement MasterContent
        {
            get { return _masterContent; }
            set
            {
                _masterContent = value;
                OnPropertyChanged();
            }
        }

        public UIElement DetailContent
        {
            get { return _detailContent; }
            set
            {
                _detailContent = value;
                OnPropertyChanged();
            }
        }

        public XViewModel MasterViewModel
        {
            get { return _masterViewModel; }
            private set { _masterViewModel = value; }
        }

        protected interface INavigationPackage
        {
            XViewModel ViewModel { get; set; }
            UIElement Page { get; set; }
            void Showing();
        }

        protected class NavigationPackage<T> : INavigationPackage where T : XViewModel
        {
            private readonly ILifetimeScope _container;

            private IXUWPFrameManager _frameManager;
                
            public NavigationPackage(ILifetimeScope container)
            {
                _container = container;

                var frameManager = _container.Resolve<IXUWPFrameManager>();
                _frameManager = frameManager;
                RootFrame = XFrame.CreateRootFrame<XUWPRootFrame>(_container);
                var rootNavigationVm = RootFrame.CreateContentModel<XUWPNavigationPageViewModel>();
                Page = frameManager.Init(RootFrame, rootNavigationVm, false);
                ViewModel = RootFrame.CreateContentModel<T>();


                RootFrame.Activated += RootFrame_Activated;
                RootFrame.Deactivated += RootFrame_Deactivated;
            }

            private void RootFrame_Deactivated(object sender, System.EventArgs e)
            {
                _frameManager.SetActive(true);
            }

            private void RootFrame_Activated(object sender, System.EventArgs e)
            {
                _frameManager.SetActive(true);
            }

            public XUWPRootFrame RootFrame { get; set; }

            public XViewModel ViewModel { get; set; }

            public UIElement Page { get; set; }
           

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
