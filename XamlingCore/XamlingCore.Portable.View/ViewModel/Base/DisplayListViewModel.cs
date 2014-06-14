using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.ViewModels;
using XamlingCore.Portable.DTO.Contract;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public abstract class DisplayListViewModel<TWrapViewModelType, TEntityType> : XViewModel
       where TWrapViewModelType : XViewModel, ISelectableItem<TEntityType>
    {

        public event EventHandler SelectionChanged;

        private TWrapViewModelType _selectedItem;
        private ObservableCollection<TWrapViewModelType> _items;

        private bool _noItems;

        protected virtual void OnItemSelected(TEntityType selectedItem)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
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

        public virtual void OnMore()
        {

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
    }
}
