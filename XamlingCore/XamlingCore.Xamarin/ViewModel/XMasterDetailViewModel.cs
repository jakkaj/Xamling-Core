using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Autofac;
using Autofac.Core;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;
using XamlingCore.XamarinThings.Frame;

namespace XamlingCore.XamarinThings.ViewModel
{
    public abstract class XMasterDetailViewModel : XViewModel
    {
        private readonly IViewResolver _viewResolver;
        private Page _masterContent;
        private Page _detailContent;

        private XViewModel _masterViewModel;

        private readonly List<XViewModel> _sectionViewModels = new List<XViewModel>();

        public Command<XViewModel> NavigateToViewModelCommand { get; set; }

        protected XMasterDetailViewModel(IViewResolver viewResolver)
        {
            _viewResolver = viewResolver;
            NavigateToViewModelCommand = new Command<XViewModel>(_onNavigateToPage);
        }

        protected void SetMaster(XViewModel masterPage)
        {
            _masterViewModel = masterPage;
        }

        protected void AddPage(XViewModel sectionPage)
        {
            SectionViewModels.Add(sectionPage);
        }

        protected void Build()
        {
            //Resolves the view and also sets the binding context
            //teh view that is associated with the view model will be used
            var masterAreaView = _viewResolver.Resolve(_masterViewModel);
            MasterContent = masterAreaView;
            
            DetailContent = _showNavPage(SectionViewModels.First());
        }

        Page _showNavPage(XViewModel vm)
        {
            var frameManager = Container.Resolve<IFrameManager>();
            var initalViewController = frameManager.Init(XFrame.CreateRootFrame<DefaultRootFrame>(Container), vm);

            return initalViewController;
        }

        void _onNavigateToPage(XViewModel navigateViewModel)
        {

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
