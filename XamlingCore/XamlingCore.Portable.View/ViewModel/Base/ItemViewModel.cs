using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Model.Contract;

namespace XamlingCore.Portable.View.ViewModel.Base
{
    public class ItemViewModel<T> : XViewModel, ISelectableItem<T> where T : class
    {
        protected T _item;

        public override void Dispose()
        {
            base.Dispose();
            _item = null;
        }

        protected virtual void OnItemChanged(T item)
        {

        }

        public T Item
        {
            get { return _item; }
            set
            {
                if (value == null)
                {
                    Debug.WriteLine("*** Missing item!");
                    return;
                }

                _item = value;
                OnPropertyChanged();
                OnItemChanged(_item);
            }
        }
    }
}
