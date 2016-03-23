using System.Collections.ObjectModel;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public abstract class SectionViewModel : XViewModel
    {
        private readonly ObservableCollection<XViewModel> _sectionViewModels = new ObservableCollection<XViewModel>();

        public XViewModel CurrentSection { get; set; }

        protected void AddPage(XViewModel sectionPage)
        {
            SectionViewModels.Add(sectionPage);
        }

        public ObservableCollection<XViewModel> SectionViewModels
        {
            get { return _sectionViewModels; }
        }
    }
}
