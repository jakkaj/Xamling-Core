using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.XamarinThings.Contract;

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

        protected void AddPage(XViewModel sectionPage)
        {
            SectionViewModels.Add(sectionPage);
        }

        protected void Build()
        {
            //Resolves the view and also sets the binding context
            //teh view that is associated with the view model will be used
            var masterAreaView = _viewResolver.Resolve(this);
            MasterContent = masterAreaView;

            var detailAreaView = _viewResolver.Resolve(SectionViewModels.First());
            DetailContent = detailAreaView;
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
