using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Modal
{
    public class ModalViewModel : XViewModel
    {
        public ICommand UnPopCommand { get; set; }

        public ModalViewModel()
        {
            UnPopCommand = new Command(_onUnPop);
        }

        void _onUnPop()
        {
            NavigateToModal(null);
        }
    }
}
