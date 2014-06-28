using System;
using Xamarin.Forms;

namespace XamlingCore.XamarinThings.Content.MasterDetail
{
    public class XMasterDetailView : MasterDetailPage
    {
        private XMasterDetailViewModel _viewModel;

        protected override void OnBindingContextChanged()
        {
            _viewModel = BindingContext as XMasterDetailViewModel;

            if (_viewModel == null)
            {
                throw new ArgumentException("BindingContext must be XMasterDetailViewModel");
            }

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;

            _setContent();
            base.OnBindingContextChanged();
        }

        void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MasterContent" || e.PropertyName == "DetailContent")
            {
                _setContent();
            }
        }

        void _setContent()
        {
            if (_viewModel.MasterContent != null && _viewModel.MasterContent != Master && Master == null)
            {
                Master = _viewModel.MasterContent;
            }

            if (_viewModel.DetailContent != null && _viewModel.DetailContent != Detail && Detail == null)
            {
                Detail = _viewModel.DetailContent;
            }
        }

        protected override void OnDisappearing()
        {
            _viewModel.PropertyChanged -= _viewModel_PropertyChanged;
            base.OnDisappearing();
        }
    }
}
