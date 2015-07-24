using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamlingCore.XamarinThings.Controls
{
    /// <summary>
    /// Helps flatten out images for Android resources (soooo annoying?!)
    /// </summary>
    public class XImage : Image
    {
        public static readonly new BindableProperty SourceProperty =
            BindableProperty.Create("Source", typeof(ImageSource), typeof(XImage), null, BindingMode.OneWay, null, _onDataContextChanged);


        public new ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        private static async void _onDataContextChanged(BindableObject obj, object oldValue, object newValue)
        {
            var thisObj = obj as XImage;

            if (thisObj == null)
            {
                return;
            }

            thisObj._setSource();
        }

        private void _setSource()
        {
            var imgSource = Source;

            if (imgSource == null)
            {
                return;
            }

            if (imgSource is FileImageSource && Device.OS == TargetPlatform.Android)
            {
                var iFile = imgSource as FileImageSource;
                var fFlattened = iFile.File.Replace('/', '_').Replace('\\', '_');
                fFlattened = fFlattened.Trim('_');
                var fs = ImageSource.FromFile(fFlattened);
                base.Source = fs;
                return;
            }

            base.Source = imgSource;
        }


    }
}
