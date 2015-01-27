using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.Portable.Contract.Services;
using XamlingCore.Portable.View.ViewModel;

namespace XamlingCore.iOS.Unified.Controls.Native
{
    public abstract class XNativePageRenderer<TFormsType, TNativeType, TViewModelType> : PageRenderer
        where TNativeType : XNativeView<TViewModelType>, new()
        where TViewModelType : XViewModel
        where TFormsType : Xamarin.Forms.VisualElement
    {
        private TFormsType _view;

        protected TViewModelType ViewModel { get; set; }


        private readonly IOrientationService _orientationService;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                ViewModel = e.NewElement.BindingContext as TViewModelType;

                ntV.SetupVm(ViewModel);

                OnControlCreated(ntV);
                ntV.OnReady();
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