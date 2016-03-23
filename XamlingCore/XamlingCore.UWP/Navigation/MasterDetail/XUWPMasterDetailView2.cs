using System;
using System.ComponentModel;
using System.ServiceModel.Channels;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using XamlingCore.Portable.Contract.Device;
using XamlingCore.Portable.Messages.View;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace XamlingCore.UWP.Navigation.MasterDetail
{
    public class XUWPMasterDetailView2 : SplitView
    {
       
        private XUWPMasterDetailViewModel _viewModel;

        public XUWPMasterDetailView2()
        {
            

            this.Register<CollapseMasterDetailMessage>(_onCollapse);
            this.Register<ShowMasterDetailMessage>(_onShow);

            this.Register<EnableMenuSwipeGesture>(_onEnableSwipe);
            this.Register<DisableMenuSwipeGesture>(_onDisableSwipe);

            this.DataContextChanged += XUWPMasterDetailView_DataContextChanged;
        }

        private void XUWPMasterDetailView_DataContextChanged(Windows.UI.Xaml.FrameworkElement sender, Windows.UI.Xaml.DataContextChangedEventArgs args)
        {
            var vm = DataContext as INotifyPropertyChanged;

            if (vm == null)
            {
                return;
            }

            vm.PropertyChanged += _viewModel_PropertyChanged;
        }

        void _onEnableSwipe()
        {
            
            
        }

        void _onDisableSwipe()
        {
           
        }

     
        void _onShow(object obj)
        {
         
        }

        void _onCollapse(object obj)
        {
           
        }

        void _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           
        }

        void _setContent()
        {
            if (_viewModel.MasterContent != null && _viewModel.MasterContent != Pane && Pane == null)
            {
                Pane = _viewModel.MasterContent;
            }

            if (_viewModel.DetailContent != null && _viewModel.DetailContent != Content)
            {
                Content = _viewModel.DetailContent;
            }

            //IsPresented = false;
        }
    }
}
