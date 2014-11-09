using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Autofac;
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

        protected XMasterDetailViewModel(IViewResolver viewResolver)
        {
            _viewResolver = viewResolver;
            NavigateToViewModelCommand = new Command<XViewModel>(_onNavigateToPage);
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
                ShowNavPage(currentVm);
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

            var firstPage = SectionViewModels.First();

            ShowNavPage(firstPage);
            

            firstPage.OnActivated();
            MasterViewModel.OnActivated();
        }

        protected virtual bool OnShowingPage(XViewModel vm)
        {
            return true;
        }

        protected void ShowNavPage(XViewModel vm)
        {
            if (!OnShowingPage(vm))
            {
                return;
            }

            var frameManager = Container.Resolve<IFrameManager>();

            var rootFrame = XFrame.CreateRootFrame<XRootFrame>(Container);
            vm.ParentModel = rootFrame; //this vm was created here, but we need to shove it to the new frame. 
            
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

            rootFrame.NavigateTo(vm);

            DetailContent = initalViewController;
            CurrentDetail = vm;
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

        
    }
}
