using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlingCore.Portable.Model.Contract
{
    public interface ISelectedItem <T>
    {
        T SelectedItem { get; set; }
        event EventHandler SelectionChanged;
    }
}
