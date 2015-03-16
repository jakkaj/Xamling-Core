using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace XamlingCore.Droid
{
    public class XApp : Xamarin.Forms.Application
    {
        public void SetMainPage(Page page)
        {
            MainPage = page;
        }
    }
}