using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;
using XamlingCore.Portable.Messages.XamlingMessenger;
using XamlingCore.Samples.Views.Root.MasterDetailRoot;
using XamlingCore.Samples.Windows8.Glue;
using XamlingCore.Windows8.Controls;
using XamlingCore.Windows8.Messages;
using NavigationEventArgs = Windows.UI.Xaml.Navigation.NavigationEventArgs;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace XamlingCore.Samples.Windows8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : WindowsPhonePage
    {
        private UserControl _loader;

        public MainPage()
        {
            this.InitializeComponent();

            var xapp = new Samples.App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);

            this.Register<SetLoaderMessage>(_onSetLoader);
            this.Register<HideLoaderMessage>(_onSetLoader);
        }

        void _onSetLoader(object message)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var m = message as SetLoaderMessage;
                if (m != null)
                {
                    var c = this.Content as Canvas;

                    if (c != null)
                    {
                        if (_loader != null)
                        {
                            var cParent = _loader.Parent as Canvas;
                            if (cParent != null) cParent.Children.Remove(_loader);
                            _loader = null;
                        }

                        var newLoader = new LoaderOverlayView();


                        _loader = newLoader;
                        c.Children.Add(newLoader);
                        newLoader.SetValue(Canvas.LeftProperty, (c.ActualWidth - newLoader.Width)/2);
                        newLoader.SetValue(Canvas.TopProperty, (c.ActualHeight - newLoader.Height)/2);

                    }
                }
                else
                {
                    if (_loader != null)
                    {
                        var cParent = _loader.Parent as Canvas;
                        if (cParent != null) cParent.Children.Remove(_loader);
                        _loader = null;
                    }
                }
                
            });

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
