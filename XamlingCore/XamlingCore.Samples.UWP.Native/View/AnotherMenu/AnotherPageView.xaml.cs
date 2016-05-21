using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XamlingCore.Samples.UWP.Native.View.Home.Subs;
using XamlingCore.UWP.Contract;
using XamlingCore.UWP.View;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace XamlingCore.Samples.UWP.Native.View.AnotherMenu
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AnotherPageView : XPage,IXPage<AnotherPageViewModel>
    {
        public AnotherPageView()
        {
            this.InitializeComponent();
        }

        public override void SetViewModel(object vm)
        {
            ViewModel = vm as AnotherPageViewModel;
        }

        public AnotherPageViewModel ViewModel { get; set; }
    }
}
