using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.iOS.Controls.Native
{
    public abstract class XNativeViewRenderer<TFormsType, TNativeType, TViewModelType> : ViewRenderer<TFormsType, TNativeType>
        where TFormsType : View
        where TNativeType : XNativeView<TViewModelType>, new()
        where TViewModelType : XViewModel
    {
        private TFormsType _view;

        protected override void OnElementChanged(ElementChangedEventArgs<TFormsType> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                if (e.NewElement != null)
                {
                    var ntV = new TNativeType();
                    ntV.SetupVm(e.NewElement.BindingContext as TViewModelType);
                    SetNativeControl(ntV);
                    OnControlCreated(ntV);
                }

                _view = e.NewElement;
            }
        }

        public virtual void OnControlCreated(TNativeType control)
        {

        }

        public TFormsType View
        {
            get { return _view; }
        }
    }
}
