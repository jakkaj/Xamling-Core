using Xamarin.Forms;

namespace XamlingCore.XamarinThings.Controls
{
    public class XSwitchCell : SwitchCell
    {
        public string TextBind
        {
            set
            {
                SetBinding(TextProperty, new Binding(value, BindingMode.OneWay));
            }
        }

        public string OnBind
        {
            set
            {
                SetBinding(OnProperty, new Binding(value, BindingMode.TwoWay));
            }
        }
    }
}
