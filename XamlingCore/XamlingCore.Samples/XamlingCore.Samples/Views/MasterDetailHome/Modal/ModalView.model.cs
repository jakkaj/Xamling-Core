using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ICommand FullScreenFiveSecCommand { get; set; }
        public ModalViewModel()
        {
            UnPopCommand = new Command(_onUnPop);
            FullScreenFiveSecCommand = new Command(_onFullScreen);
            NextPageCommand = new Command(_onNextPage);
            Title = "First modal view";
        }

        async void _onFullScreen()
        {
            var result = await Loader(_fiveSecondTask(), "Doing something...");

            Debug.WriteLine("The result is: {0}", result);
        }

        async Task<bool> _fiveSecondTask()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            return true;
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
