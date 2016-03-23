using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using XamlingCore.Portable.Contract.ViewModels;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public abstract class DisplayListViewModel<TWrapViewModelType, TEntityType> : XViewModel, 
        IDataListViewModel<TEntityType>, ISelectedItem<TWrapViewModelType>, ISelectableItem<TEntityType>
       where TWrapViewModelType : XViewModel, ISelectableItem<TEntityType>
    {

        public event EventHandler SelectionChanged;

        private TWrapViewModelType _selectedItem;
        private ObservableCollection<TWrapViewModelType> _items;

        //this is so the parent VM can pass data in. This is not mandatory (the supers might like to get their own data)
        private ObservableCollection<TEntityType> _dataList; 

        private bool _noItems;

        public ICommand MoreDataCommand { get; set; }

        public DisplayListViewModel()
        {
            MoreDataCommand = new XCommand<object>(OnMore);
        }

        protected virtual void OnItemSelected(TEntityType selectedItem)
        {
            Item = selectedItem;

            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }

        protected void UpdateItemCount()
        {
            NoItems = Items == null || Items.Count == 0;
        }

        void _onItemChanged()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem is IMoreTileViewModel)
                {
                    OnMore();
                }
                else
                {
                    OnItemSelected(SelectedItem.Item);
                }
            }
        }

        public virtual void OnItemsChanged()
        {
            
        }

        public virtual void OnMore()
        {

            OnItemsChanged();
        }

        public TWrapViewModelType SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                _onItemChanged();
            }
        }

        public ObservableCollection<TWrapViewModelType> Items
        {
            get { return _items; }
            set
            {
                _items = value;

                NoItems = Items == null || Items.Count == 0;

                OnPropertyChanged();
                OnItemsChanged();
            }
        }

        public bool NoItems
        {
            get { return _noItems; }
            set
            {
                _noItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TEntityType> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value; 
                OnPropertyChanged();
            }
        }

        public TEntityType Item { get; private set; }
    }
}
