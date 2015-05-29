//using System.Threading.Tasks;
//using Windows.UI;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Media;
//using Xamarin.Forms.Platform.WinRT;
//using XamlingCore.Samples.Windows8.Renderer;
//using XamlingCore.XamarinThings.Content.Navigation;
//using Color = Xamarin.Forms.Color;

//[assembly: ExportRenderer(typeof(XNavigationPageView), typeof(WindowsNavigationPageRenderer))]
//namespace XamlingCore.Samples.Windows8.Renderer
//{
//    public class WindowsNavigationPageRenderer : NavigationPageRenderer
//    {
//        public WindowsNavigationPageRenderer()
//        {
//            _wait();
//        }

//        async void _wait()
//        {
//            while (this.ContainerElement == null)
//            {
//                await Task.Yield();
//            }

//            var ele = this.ContainerElement as PageControl;

//            var c = ele.Parent as Canvas;
//            if (c != null)
//            {
//                c.Background = new SolidColorBrush(Colors.Blue);
//                ele.Background = new SolidColorBrush(Colors.Red);
                
//                this.Element.BackgroundColor = Color.Transparent;
                
//            }
//            //var c2 = c.Parent;

            

            
//           // c2.Background = new SolidColorBrush(Colors.Green);
//        }
//    }
//}