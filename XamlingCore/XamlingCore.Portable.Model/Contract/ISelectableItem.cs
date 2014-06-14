using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.DTO.Contract
{
    public interface ISelectableItem<T>
    {
        T Item { get; }
    }
}
