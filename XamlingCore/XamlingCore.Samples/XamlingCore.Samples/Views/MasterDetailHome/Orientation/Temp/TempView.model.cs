using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Orientation.Temp
{
    public class TempViewModel : XViewModel
    {
        public async override void OnActivated()
        {
            await Task.Delay(1000);
            NavigateBack();
            base.OnActivated();
        }
    }
}
