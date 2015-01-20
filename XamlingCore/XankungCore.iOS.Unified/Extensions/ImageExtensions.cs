//Special thanks to Xamarin
//https://github.com/xamarin/prebuilt-apps/blob/master/FieldService/FieldService.iOS/Utilities/UIKitExtensions.cs

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace XamlingCore.iOS.Unified.Extensions
{
    public static class ImageExtensions
    {
        public static UIImage ToUIImage(this System.IO.Stream stream)
        {
            if (stream == null)
                return null;

            using (var data = NSData.FromStream(stream))
            {
                return UIImage.LoadFromData(data);
            }
        }

        public async static Task<UIImage> ToUIImage(this ImageSource source)
        {
            if (source == null)
                return null;

            var bitMap = source as StreamImageSource;

            if (bitMap == null)
            {
                return null;
            }
            using (var result = await bitMap.Stream(new CancellationToken()))
            {
                return result.ToUIImage();
            }
        }

        /// <summary>
        /// Converts a UIImage to a byte array
        /// </summary>
        public static byte[] ToByteArray(this UIImage image)
        {
            if (image == null)
                return null;

            using (image)
            {
                using (var data = image.AsJPEG())
                {
                    var bytes = new byte[data.Length];
                    Marshal.Copy(data.Bytes, bytes, 0, (int)data.Length);
                    return bytes;
                }
            }
        }
    }
}
