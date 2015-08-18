using Xamarin.Forms;

namespace XamlingCore.Samples.Views.MasterDetailHome.List
{
    public class LongListItemView : ContentView
    {
        public LongListItemView()
        {
            var l = new Label();
            l.SetBinding(Label.TextProperty, "Text");

            Content = l;
        }

        protected override void OnBindingContextChanged()
        {
            //var b = BindingContext as LongListItemViewModel;
            //if (b != null)
            //{
            //    Debug.WriteLine("Loaded: " + b.Text);
            //}
            base.OnBindingContextChanged();
        }
    }
}
