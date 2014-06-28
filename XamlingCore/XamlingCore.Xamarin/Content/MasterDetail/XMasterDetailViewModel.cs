using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autofac;
using Xamarin.Forms;
using XamlingCore.Portable.Contract.ViewModels;
using XamlingCore.Portable.Model.Contract;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.XamarinThings.Content.Navigation;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.XamarinThings.Content.MasterDetail
{
    public abstract class XMasterDetailViewModel : XViewModel
    {
        private readonly IViewResolver _viewResolver;
        private Page _masterContent;
        private Page _detailContent;

        private XViewModel _masterViewModel;

        private readonly List<XViewModel> _sectionViewModels = new List<XViewModel>();

        public Command<XViewModel> NavigateToViewModelCommand { get; set; }

        public Color? NavigationTint { get; set; }

        protected XMasterDetailViewModel(IViewResolver viewResolver)
        {
            _viewResolver = viewResolver;
            NavigateToViewModelCommand = new Command<XViewModel>(_onNavigateToPage);
        }

        protected void SetMaster(XViewModel masterPage)
        {
            _masterViewModel = masterPage;

            var selectable = _masterViewModel as ISelectableItem<XViewModel>;
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
                _showNavPage(currentVm);
            }
        }

        protected void AddPage(XViewModel sectionPage)
        {
            SectionViewModels.Add(sectionPage);
        }

        protected void Build()
        {
            var v = _masterViewModel as IDataListViewModel<XViewModel>;

            if (v == null)
            {
                throw new ArgumentException("Master view model must implement IItemViewModel<XViewModel> so the root master can pass in items to show");
            }

            v.DataList = new ObservableCollection<XViewModel>(_sectionViewModels);

            //Resolves the view and also sets the binding context
            //teh view that is associated with the view model will be used
            var masterAreaView = _viewResolver.Resolve(_masterViewModel);
            MasterContent = masterAreaView;

            var firstPage = SectionViewModels.First();

            _showNavPage(firstPage);
            

            firstPage.OnActivated();
            _masterViewModel.OnActivated();
        }

        void _showNavPage(XViewModel vm)
        {
            var frameManager = Container.Resolve<IFrameManager>();

            var rootFrame = XFrame.CreateRootFrame<XRootFrame>(Container);
            vm.ParentModel = rootFrame; //this vm was created here, but we need to shove it to the new frame. 

            rootFrame.NavigateTo(vm);

            var rootNavigationVm = rootFrame.CreateContentModel<XNavigationPageViewModel>();

            var initalViewController = frameManager.Init(rootFrame, rootNavigationVm);

            if (NavigationTint != null)
            {
                var navPage = initalViewController as NavigationPage;
                if (navPage != null)
                {
                    navPage.Tint = NavigationTint.Value;
                }
            }

            DetailContent = initalViewController;
        }

        void _onNavigateToPage(XViewModel navigateViewModel)
        {

        }

        public override void Dispose()
        {
            if (_masterViewModel == null) return;

            var selectable = _masterViewModel as ISelectableItem<XViewModel>;
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

        public List<XViewModel> SectionViewModels
        {
            get { return _sectionViewModels; }
        }
    }
}
