using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.XamarinThings.Content.Common
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
