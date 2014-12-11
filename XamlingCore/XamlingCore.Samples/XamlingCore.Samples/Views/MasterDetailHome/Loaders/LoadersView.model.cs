using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.Samples.Views.MasterDetailHome.Loaders
{
    public class LoadersViewModel : XViewModel
    {
        public ICommand FullScreenFiveSecCommand { get; set; }
        public ICommand TrayAddCommand { get; set; }

        public ICommand NavigateBackCommand { get; set; }

        public ICommand ModalCommand { get; set; }

        public LoadersViewModel()
        {
            Title = "Loaders";
            FullScreenFiveSecCommand = new Command(_onFullScreen);
            TrayAddCommand = new Command(_onTrayAdd);
            NavigateBackCommand = new Command(_onNavigateBack);
            ModalCommand = new Command(_onModal);
        }

        void _onNavigateBack()
        {
            NavigateBack();
        }

        void _onModal()
        {
            NavigateToModal(CreateContentModel<LoadersViewModel>());
        }

        async void _onFullScreen()
        {
            var result  = await Loader(_fiveSecondTask(), "Doing something...");

            Debug.WriteLine("The result is: {0}", result);
        }

        async Task<bool> _fiveSecondTask()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            return true;
        }

        async void _onTrayAdd()
        {
            var result = await Loader(_fiveSecondTask());

            Debug.WriteLine("The result is: {0}", result);
        }

        
    }

    
}
