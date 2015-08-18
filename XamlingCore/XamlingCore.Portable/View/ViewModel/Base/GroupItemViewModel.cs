using System.Collections.ObjectModel;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public class GroupItemViewModel : ObservableCollection<XViewModel>
    {
        public string GroupName { get; set; }
    }
}
