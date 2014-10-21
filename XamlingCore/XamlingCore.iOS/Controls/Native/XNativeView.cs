using System;
using System.Drawing;
using MonoTouch.UIKit;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.iOS.Controls.Native
{
    public abstract class XNativeView<T> : UIView where T : XViewModel
    {
        private T _vm;

        protected XNativeView()
        {

        }

        protected XNativeView(RectangleF bounds)
            : base(bounds)
        {

        }

        public void SetupVm(T vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(string.Format("Expected ViewModel type of: {0}", typeof(T).FullName));
            }

            _vm = vm;

            _vm.PropertyChanged += _vm_PropertyChanged;

            OnInitialise();
        }

        void _vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnViewModelPropertyChanged(e.PropertyName);
        }

        protected virtual void OnViewModelPropertyChanged(string propertyName)
        {

        }

        public T ViewModel { get { return _vm; } }


        protected abstract void OnInitialise();
    }
}
