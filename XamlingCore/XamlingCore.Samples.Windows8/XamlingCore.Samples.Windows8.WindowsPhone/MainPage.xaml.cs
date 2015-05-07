using Windows.ApplicationModel.Activation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;
using XamlingCore.Samples.Windows8.Glue;
using NavigationEventArgs = Windows.UI.Xaml.Navigation.NavigationEventArgs;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace XamlingCore.Samples.Windows8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : WindowsPhonePage
    {
        public MainPage()
        {
            this.InitializeComponent();
               
            var xapp = new Samples.App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
    }
}
