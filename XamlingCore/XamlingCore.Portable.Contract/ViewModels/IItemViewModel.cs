using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Contract.ViewModels
{
    public interface IDataListViewModel<T>
    {
        ObservableCollection<T> DataList { get; set; }
    }
}
