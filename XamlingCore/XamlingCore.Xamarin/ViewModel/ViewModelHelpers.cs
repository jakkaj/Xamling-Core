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
            if (obj.BindingContext == null)
            {
                throw new ArgumentNullException("No binding context present in ViewModel cast");
            }

            if (!(obj.BindingContext is TViewModel))
            {
                throw new InvalidCastException(string.Format("ViewModel cast fail. Asked for {0} but sent in {1}", typeof(TViewModel).Name, obj.BindingContext.GetType().Name));
            }
            return obj.BindingContext as TViewModel;
        }
    }
}
