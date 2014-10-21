using System.Threading;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace XamlingCore.iOS.Extensions
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
    }
}
