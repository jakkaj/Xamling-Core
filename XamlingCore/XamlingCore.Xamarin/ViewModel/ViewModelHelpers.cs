using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.XamarinThings.ViewModel
{
    public static class ViewModelHelpers
    {
        public static TViewModel ViewModel<TViewModel>(this BindableObject obj)
            where TViewModel : XViewModel
        {
            return obj.BindingContext as TViewModel;
        }
    }
}
