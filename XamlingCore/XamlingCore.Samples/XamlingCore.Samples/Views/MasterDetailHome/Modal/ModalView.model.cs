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
        public ICommand NextPageCommand { get; set; }

        public ModalViewModel()
        {
            UnPopCommand = new Command(_onUnPop);
            NextPageCommand = new Command(_onNextPage);
            Title = "First modal view";
        }

        void _onNextPage()
        {
            NavigateTo<ModalAnotherViewModel>();    
        }

        void _onUnPop()
        {
            NavigateBack();
            //NavigateToModal(null);
        }
    }
}
