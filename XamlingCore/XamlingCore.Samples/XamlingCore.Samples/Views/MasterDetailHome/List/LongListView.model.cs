using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.Util.Lock;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.List
{
    public class LongListViewModel : XViewModel
    {
        ObservableCollection<XViewModel> _items = new ObservableCollection<XViewModel>();

        public ICommand MoreDataCommand { get; set; }

        private const int pageSize = 100;

        public LongListViewModel()
        {
            Title = "Long lists";
            MoreDataCommand = new Command(_onNeedMoreData);
        }

        private async void _onNeedMoreData()
        {
            if (_items.LastOrDefault() is InPlaceLoaderViewModel)
            {
                return;
            }
            
            var a = CreateContentModel<InPlaceLoaderViewModel>();
            _items.Add(a);
            await Task.Delay(3000);
            _items.Remove(a);
            for (var i = 0; i < pageSize; i++)
            {
                var c = _items.Count;
                var vm = CreateContentModel<LongListItemViewModel>(_ => _.Text = "Text: " + i + "_" + c);
                _items.Add(vm);
            }
        }


        public override void OnInitialise()
        {
            for (var i = 0; i < pageSize; i++)
            {
                var vm = CreateContentModel<LongListItemViewModel>(_ => _.Text = "Text: " + i);
                _items.Add(vm);
            }

            base.OnInitialise();
        }

        public ObservableCollection<XViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }
    }
}
