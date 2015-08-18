using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public abstract class GroupListViewModel<TWrapViewModelType, TEntityType> : XViewModel
         where TWrapViewModelType : XViewModel, ISelectableItem<TEntityType>
    {

        public event EventHandler SelectionChanged;

        private TWrapViewModelType _selectedItem;
        private ObservableCollection<GroupItemViewModel> _items = new ObservableCollection<GroupItemViewModel>();

        private bool _noItems;
        private bool _isReady;

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
                OnItemSelected(SelectedItem.Item);
            }
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

        public ObservableCollection<GroupItemViewModel> Items
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

        public bool IsReady
        {
            get { return _isReady; }

            protected set
            {
                _isReady = value;
            }
        }

        public event EventHandler Ready;

        protected void NotifyReady()
        {
            _isReady = true;
            if (Ready != null)
            {
                Ready(this, EventArgs.Empty);
            }
        }
    }
}
