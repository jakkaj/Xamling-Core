using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.XamarinThings.Content.Navigation
{
    public class XNavigationPageTypedViewModel <TInitialViewModel> : XViewModel
        where TInitialViewModel : XViewModel
    {
        public override void OnInitialise()
        {
            NavigateTo<TInitialViewModel>();
            base.OnInitialise();
        }
    }
}
