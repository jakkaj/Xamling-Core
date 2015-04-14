using Xamarin.Forms.Platform.WinRT;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;
using XamlingCore.Samples.Windows8.Glue;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace XamlingCore.Samples.Windows8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();


            var xapp = new Samples.App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);
            
        }

       

        void _init()
        {
            //var core = new XWindows8Core<Windows8RootFrame, HomeViewModel, ProjectGlue>();
            //core.Init();
            //core.SetRootFrame(RootWindowsFrame);
        }
    }
}
