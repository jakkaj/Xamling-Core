using Xamarin.Forms;

namespace XamlingCore.Samples.Views.MasterDetailHome.Home
{
    public partial class HomePageTwoView : ContentPage
    {
        public HomePageTwoView()
        {  
            InitializeComponent();
            Title = "HomePageTwo";

            //var root = this.Content as StackLayout;
            //var transition = new TransitionContentView(this);
            //transition.Duration = .5;
            //transition.SetBinding(TransitionContentView.DataContextProperty, "DynamicViewModel");
            //root.Children.Add(transition);
        }
    }
}
