using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using XamlingCore.Windows8.View;

namespace XamlingCore.Windows8.Contract
{
    public interface IUWPViewResolver
    {
        XPage Resolve(object content);
        Type ResolvePageType(object content);
    }
}
