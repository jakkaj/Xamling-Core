using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.View.Properties;

namespace XamlingCore.XamarinThings.Contract
{
    public interface IIconView
    {
        FileImageSource Icon { get; set; }
    }
}
