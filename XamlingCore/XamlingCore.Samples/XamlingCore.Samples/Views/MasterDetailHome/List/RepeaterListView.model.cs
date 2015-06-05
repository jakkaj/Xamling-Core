using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Portable.View.ViewModel.Base;
using XamlingCore.Samples.Views.MasterDetailHome.Home.ListContent;

namespace XamlingCore.Samples.Views.MasterDetailHome.List
{
    public class RepeaterListViewModel : XViewModel
    {
        private ObservableCollection<XViewModel> _items;

        public RepeaterListViewModel()
        {
            Items = new GroupItemViewModel();
        }

        public override void OnInitialise()
        {
            base.OnInitialise();
            _loop();
        }

        private async void _loop()
        {
            while (true)
            {
                _loadItem();
                await Task.Delay(1000);
            }
        }

        void _loadItem()
        {
            var i = CreateContentModel<SomeViewModel>(_=>_.Title = DateTime.Now.ToString());

            Items.Add(i);

            while (Items.Count > 4)
            {
                Items.RemoveAt(0);
            }
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
