using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Portable.View.ViewModel;
using XamlingCore.Samples.Views.MasterDetailHome.AnotherMenuOption;
using XamlingCore.Samples.Views.MasterDetailHome.Home.DynamicContentExamples;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public class HomePageTwoViewModel : XViewModel
    {
        public ICommand PopCommand { get; set; }

        public ICommand NextCommand { get; set; }

        private XViewModel _dynamicViewModel;

        public HomePageTwoViewModel()
        {
            PopCommand = new Command(_onPop);
            NextCommand = new Command(_onNext);
           
        }


        public override void OnActivated()
        {
            _doThings();
            base.OnActivated();
        }

        void _onNext()
        {
            NavigateTo<HomePageThreeViewModel>(null, true);
        }

        void _onPop()
        {
            NavigateToModal(null);
        }


        async void _doThings()
        {

            int count = 0;

            await Task.Delay(4000);

            DynamicViewModel = CreateContentModel<FirstDynamicViewModel>();

            while (count < 3)
            {
                await Task.Delay(3000);
                if (DynamicViewModel is FirstDynamicViewModel)
                {
                    DynamicViewModel = CreateContentModel<SecondDynamicViewModel>();
                }
                else
                {
                    DynamicViewModel = CreateContentModel<FirstDynamicViewModel>();
                }
                count++;


            }




        }

        public XViewModel DynamicViewModel
        {
            get { return _dynamicViewModel; }
            set
            {
                _dynamicViewModel = value;
                OnPropertyChanged();
            }
        }
    }
}
